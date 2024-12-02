import { patchState, signalStore, withHooks, withMethods, withState } from "@ngrx/signals";
import { User } from "../domain/user";
import { AuthService } from "@auth0/auth0-angular";
import { inject } from "@angular/core";
import { tap, switchMap } from "rxjs";

export type UserState = {
  user: User,
}

const initialUserState: UserState = {
  user: {
    loggedin: false,
    email: "",
    contexts: [],
    referenceLang: undefined,
    name: "unknown",
    picture: ""
  },
}

export const UserStore = signalStore(
  { providedIn: 'root' },
  withState(initialUserState),

  withMethods((store, service = inject(AuthService)) => ({
    login: () => {
      service.loginWithRedirect().subscribe({
        next: (value) => {
          console.log("pix login", value)
          patchState(store, { user: { ...store.user(), loggedin: true } });
        },
        error: (err) => console.error(err),
      });
    }
  })),

  withHooks({
    onInit: (store, service = inject(AuthService)) => {
      service.isAuthenticated$
        .pipe(
          tap({
            next: (v) => patchState(store, { user: { ...store.user(), loggedin: v } }),
            error: console.error,
          }),
          switchMap(_ => service.user$),
          tap({
            next: (v: any) => {
              patchState(store, { user: { ...store.user(), name: v.name, email: v.email, picture: v.picture } })
            },
            error: console.error
          })
        )
        .subscribe();
    }
  })
);
