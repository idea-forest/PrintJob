import { BackgroundProps, TextProps } from '@chakra-ui/react';
import { StaticImageData } from 'next/image';
import { ReactElement, ReactNode } from 'react';

export interface LayoutProps {
  children?: ReactNode;
}

export interface IProps {
  title?: string;
  desc?: ReactNode;
}

export interface NewPageItemsModel extends IProps {
  id: string;
  icon: StaticImageData;
}

export interface ActionTitleModel {
  header?: string;
  headerColor: string;
  icon: StaticImageData;
  textProps?: TextProps;
}

export interface ActionCardModel extends IProps, ActionTitleModel {
  isShowTopBtn?: boolean;
  btnText: string;
  onClick?: () => void;
  path: string;
  gradientImg?: StaticImageData;
  atEdge?: boolean;
  leftIcon?: ReactElement;
  btnBg?: BackgroundProps['bg'];
  details?: ReactNode;
  showTexts?: boolean;
  info?: string;
}


export interface TopBarProps {
  header?: ReactNode;
  showBckBtn?: boolean;
  otherBtn?: ReactNode;
}


export interface ColorsModel {
  color: string;
  bg: string;
}

export interface Reset {
  reset: () => void;
}


export type AlertStatus = 'error' | 'success' | 'warning' | 'info';
