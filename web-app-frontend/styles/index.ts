import { Outfit } from 'next/font/google';

export * from './theme';
export * from './globalStyles';

export const outFit = Outfit({
  weight: ['400', '500', '600', '700', '800', '900'],
  subsets: ['latin'],
});
