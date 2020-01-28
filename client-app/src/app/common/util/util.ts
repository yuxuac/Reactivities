import { IActivity, IAttendee } from "../../Models/activity";
import { IUser } from "../../Models/user";

export const combineDateAndTime = (date: Date, time: Date) => {
  // const timeString = time.getHours() + ":" + time.getMinutes() + ":00";

  // const year = date.getFullYear();
  // const month = date.getMonth() + 1;
  // const day = date.getDate();
  // const dateString = `${year}-${month}-${day}`;

  const dateString = date.toISOString().split('T')[0];
  const timeString = date.toISOString().split('T')[1];
  return new Date(dateString + "T" + timeString);
};

export const setActivityProps = (activity: IActivity, user: IUser) => {
  activity.date = new Date(activity.date);
  activity.isGoing = activity.attendees.some(a => a.username === user.username);
  activity.isHost = activity.attendees.some(
    a => a.username === user.username && a.isHost
  );
  return activity;
};

export const createAttendee = (user: IUser): IAttendee => {
  return {
    displayName: user.displayName,
    isHost: false,
    username: user.username,
    image: user.image!
  };
};

// export const sleep = (ms: number) => {
//   return new Promise(resolve => setTimeout(resolve, ms));
// };

// export const assureHubStableStatus = async (hub: HubConnection | null) => {
//   if (!hub) throw new Error("there is no hub");

//   while (
//     hub.state === "Connecting" ||
//     hub.state === "Disconnecting" ||
//     hub.state === "Reconnecting"
//   ) {
//     console.log("Wait to (dis)connect, now state is:" + hub.state);
//     await sleep(1000);
//   }

//   console.log("Now connected");
// };
