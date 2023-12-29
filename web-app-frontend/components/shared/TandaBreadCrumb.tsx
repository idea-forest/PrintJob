import { DEFAULT_STYLES } from 'styles';
import { DashRoutes } from 'utils';
import { Breadcrumb, BreadcrumbItem, BreadcrumbLink } from '@chakra-ui/react';
import { useRouter } from 'next/router';

export const TandaBreadCrumb = () => {
  const router = useRouter();
  const paths = router.pathname.split('/').slice(1);

  const renderBreadCrumItems =
    paths.length > 1 &&
    paths.map((path, i) => {
      const isCurrent = i === paths.length - 1;
      const route =
        path === 'dashboard'
          ? DashRoutes.dashboard
          : `${DashRoutes.dashboard}/${path}`;

      return (
        <BreadcrumbItem key={`${path}_${i}`}>
          <BreadcrumbLink
            isCurrentPage={isCurrent}
            textTransform='capitalize'
            fontSize={DEFAULT_STYLES.desc}
            href={route}
            color={isCurrent ? DEFAULT_STYLES.primaryColor : 'inherit'}
            _hover={{
              textDecoration: 'none',
              opacity: 0.5,
            }}
          >
            {path}
          </BreadcrumbLink>
        </BreadcrumbItem>
      );
    });

  return (
    <Breadcrumb alignSelf='flex-start' px='2' pb='2'>
      {renderBreadCrumItems}
    </Breadcrumb>
  );
};
