import { DEFAULT_STYLES } from 'styles';
import { DashRoutes } from 'utils';
import { useRouter } from 'next/router';

export const useSharedColor = () => {
  const router = useRouter();
  const isExtend = router.pathname === DashRoutes.financeExtend;

  return isExtend ? DEFAULT_STYLES.pink : DEFAULT_STYLES.lightPurple;
};
