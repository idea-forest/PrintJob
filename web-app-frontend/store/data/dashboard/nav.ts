import { NavItemProps } from 'models';
import { DashRoutes } from 'utils';
import { LuPrinter } from "react-icons/lu";
import { TbDeviceIpadShare } from "react-icons/tb";
import { nanoid } from 'nanoid';

export const deviceItems: NavItemProps[] = [
  {
    icon: TbDeviceIpadShare,
    title: 'Device Management',
    path: DashRoutes.deviceList,
    id: nanoid(),
  },
  {
    icon: LuPrinter,
    title: 'Print Jobs',
    path: DashRoutes.printJobsList,
    id: nanoid(),
  },
];