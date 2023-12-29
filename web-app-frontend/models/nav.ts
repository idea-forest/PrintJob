import { IconType } from 'react-icons/lib';

export interface NavItemProps {
  icon: IconType;
  title: string;
  path?: string;
  id: string;
}