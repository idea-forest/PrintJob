import financeIcon from 'assets/images/finance.png';
import bankIcon from 'assets/images/bank.png';
import extendIcon from 'assets/images/extend.png';
import growIcon from 'assets/images/grow.png';
import fairerIcon from 'assets/images/fairer.png';
import fasterIcon from 'assets/images/faster.png';
import maskIcon from 'assets/images/mask.png';
import plusIcon from 'assets/images/gradientplus.png';
import party from 'assets/images/party.png';
import extendcurve from 'assets/images/extendcurve.png';
import growcurve from 'assets/images/growcurve.png';
import { NewPageItemsModel } from 'models';
import { nanoid } from 'nanoid';

export {
  financeIcon,
  bankIcon,
  extendIcon,
  growIcon,
  plusIcon,
  party,
  extendcurve,
  growcurve,
};

export const newPageItems: NewPageItemsModel[] = [
  {
    title: 'Faster',
    desc: `The average term sheet
    takes over 30 meetings to
    get. You can get yours the
    next day.`,
    id: nanoid(12),
    icon: fasterIcon,
  },
  {
    title: 'Fairer',
    desc: `The average average founding team’s stake at exit is <15%. With Tanda yours will be higher.`,
    id: nanoid(12),
    icon: fairerIcon,
  },
  {
    title: 'More flexible',
    desc: `We partner directly with you to ensure you make the right decisions for your business.`,
    id: nanoid(12),
    icon: maskIcon,
  },
];

export const financeDetailsHeaders = [
  { name: 'Name' },
  { name: 'Date deployed' },
  { name: 'Maturity' },
  { name: 'Total Outstanding', other: 'Total' },
  { name: 'Outstanding Principal', other: 'Total Principal' },
  { name: 'Outstanding Fee', other: 'Total fee' },
];

export const financeDetailsBody = [
  {
    name: 'First Draw',
  },
  { name: 'Feb 23,2023' },
  { name: 'Pending' },
  { name: '$46633438', other: 'of $46633438' },
  { name: '$46633438', other: ' of $46633438' },
  { name: '$46633438', other: ' of $46633438' },
];
