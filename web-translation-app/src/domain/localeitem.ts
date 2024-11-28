import { z } from 'zod';

export const ContextSchema = z.object({
  id: z.string(),
  name: z.string(),
});

export type Context = z.infer<typeof ContextSchema>;


const LocaleItemSchema = z.object({
  contextId: z.string(),
  lang: z.string(),
  content: z.string(),
});

export type LocaleItem = z.infer<typeof LocaleItemSchema>;
