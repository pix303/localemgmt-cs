import { Injectable } from '@angular/core';
import { LocaleItem } from '../domain/localeitem';

@Injectable({ providedIn: 'root' })
export class LocaleItemService {

  async getLocaleItems(): Promise<LocaleItem[]> {
    return new Promise((resolve) => {
      setTimeout(() => {
        resolve(mokeupData);
      }, 1000);
    });
  }
}

export const mokeupData = [
  { lang: "it", contextId: "default", content: "ciao come va" },
  { lang: "en", contextId: "default", content: "hello how are you" },
  { lang: "it", contextId: "default", content: "ciao ciao ciao me va" },
];
