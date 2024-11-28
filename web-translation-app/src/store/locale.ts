import { computed, effect, inject } from "@angular/core";
import { LocaleItem } from "../domain/localeitem";
import { User } from "../domain/user";
import { getState, patchState, signalStore, withComputed, withHooks, withMethods, withState } from "@ngrx/signals";
import { LocaleItemService } from "../service/localeitem.service";

type FormValue<T> = T | null | undefined;

export type LocaleState = {
  _loadedItems: LocaleItem[],
  user: User | undefined,
}

export type LocaleStateMetadata = {
  _filters: {
    context?: FormValue<string>,
    lang?: FormValue<string>,
    content?: FormValue<string>
  },
  isLoading: boolean,
  error: string | undefined,
  warning: string | undefined,
}

const initialState: LocaleState = {
  _loadedItems: [],
  user: undefined,
}

const initialMetadata: LocaleStateMetadata = {
  _filters: {},
  isLoading: false,
  error: undefined,
  warning: undefined,
}

export const LocaleStore = signalStore(
  { providedIn: 'root' },
  withState(initialState),
  withState(initialMetadata),

  withComputed(({ _loadedItems, _filters }) => ({
    items: computed(() => {
      const { content, context, lang } = _filters();
      if (!content && !context && !lang) return _loadedItems();

      const result = _loadedItems().filter(item => {
        if (content && item.content.toLowerCase().includes(content)
          || context && item.contextId === context
          || lang && item.lang === lang
        ) {
          return true;
        }
        return false;
      });
      return result;
    }),
  })),

  withMethods((store, service = inject(LocaleItemService)) => ({
    loadItems: async () => {
      const result = await service.getLocaleItems();
      patchState(store, { _loadedItems: result });
    },
    setFilters: (lang: FormValue<string>, content: FormValue<string>) => {
      patchState(store, { _filters: { content, lang } });
    }
  })),

  withHooks({
    onInit(store) {
      store.loadItems();
      effect(() => {
        const currentState = getState(store);
        //console.debug("[pix-store-state]", currentState);
      })
    },
  })
);
