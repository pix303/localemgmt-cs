import { ContextSchema } from "./localeitem";
import { z } from 'zod';


export const UserSchema = z.object({
  id: z.string().email(),
  contexts: z.array(ContextSchema),
  referenceLang: z.string(),
});

export type User = z.infer<typeof UserSchema>;
