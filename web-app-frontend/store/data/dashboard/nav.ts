import { NavItemProps } from 'models';
import { DashRoutes } from 'utils';
import { TfiMenuAlt } from 'react-icons/tfi';
// import { ImStack } from 'react-icons/im';
// import { MdStackedLineChart } from 'react-icons/md';
// import { FiCheckCircle } from 'react-icons/fi';
// import { HiOutlineUsers } from 'react-icons/hi';
// import { BsCreditCard, BsCashStack } from 'react-icons/bs';
import { nanoid } from 'nanoid';

export const deviceItems: NavItemProps[] = [
  {
    icon: TfiMenuAlt,
    title: 'Device Management',
    path: DashRoutes.device,
    id: nanoid(),
  },
];