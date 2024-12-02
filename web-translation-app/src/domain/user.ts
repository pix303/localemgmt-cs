import { ContextSchema } from "./localeitem";
import { z } from 'zod';


export const UserSchema = z.object({
  loggedin: z.boolean(),
  email: z.string().email(),
  name: z.string(),
  picture: z.string(),
  contexts: z.array(ContextSchema),
  referenceLang: z.string().optional(),
});

export type User = z.infer<typeof UserSchema>;
