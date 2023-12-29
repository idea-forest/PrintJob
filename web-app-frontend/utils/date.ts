type DateStyle = 'medium' | 'long' | 'full' | 'short';

const formatDate = (dateParser: Intl.DateTimeFormat, date?: Date) => {
  const time = date ? new Date(date) : new Date();
  const formatted = dateParser.format(time);
  return formatted;
};

export const getDay = (type: DateStyle, date?: Date) => {
  const dateParser = new Intl.DateTimeFormat('en-US', {
    dateStyle: type,
  });

  return formatDate(dateParser, date);
};
