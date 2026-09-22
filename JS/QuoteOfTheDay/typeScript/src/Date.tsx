import dayjs from "dayjs";
import './css/Date.css';


export default function Date() {
    const currentDate = dayjs().format('dddd, MMMM D, YYYY');
  return (
      <time>{currentDate}</time>
  )
}
