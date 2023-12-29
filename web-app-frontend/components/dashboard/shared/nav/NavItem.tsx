import Link from 'next/link';
import { DEFAULT_STYLES } from 'styles';
import { Flex, Icon } from '@chakra-ui/react';
import { useRouter } from 'next/router';
import { IconType } from 'react-icons/lib';
import { Desc } from 'components/shared';

interface Props {
  icon: IconType;
  title: string;
  path?: string;
  onClick?: () => void;
}

export const NavItem = ({ icon, title, path, onClick }: Props) => {
  const { pathname } = useRouter();
  const isActive = path === pathname;

  const navItem = (
    <Flex
      w={DEFAULT_STYLES.fullWidth}
      pl={DEFAULT_STYLES.sidePl}
      cursor='pointer'
      onClick={onClick}
      borderRadius={5}
      bg={isActive ? 'rgba(110, 49, 240, 0.1)' : 'transparent'}
      gap='2'
      py='2'
      role='group'
      _hover={{
        bg: 'rgba(110, 49, 240, 0.1)',
      }}
      align='center'
    >
      <Icon
        as={icon}
        color={isActive ? DEFAULT_STYLES.lightPurple : DEFAULT_STYLES.darkGray}
        _groupHover={{
          color: DEFAULT_STYLES.lightPurple,
        }}
      />

      <Desc
        color={isActive ? DEFAULT_STYLES.lightPurple : DEFAULT_STYLES.darkGray}
        _groupHover={{
          color: DEFAULT_STYLES.lightPurple,
        }}
      >
        {title}
      </Desc>
    </Flex>
  );

  return <>{path ? <Link href={path}>{navItem}</Link> : navItem}</>;
};
