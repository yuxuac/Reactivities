import React from "react";
import { Tab } from "semantic-ui-react";
import ProfilePhotos from "./ProfilePhotos";
import ProfileAbout from "./ProfileAbout";
import ProfileFollowings from "./ProfileFollowings";
import { observer } from "mobx-react-lite";
import ProfileActivities from "./ProfileActivities";

const panes = [
  { menuItem: "About", render: () => <ProfileAbout /> },
  { menuItem: "Photos", render: () => <ProfilePhotos /> },
  {
    menuItem: "Activities",
    render: () => <ProfileActivities />
  },
  {
    menuItem: "Followers",
    render: () => <ProfileFollowings />
  },
  {
    menuItem: "Following",
    render: () => <ProfileFollowings />
  }
];

interface IProps {
  setActiveTab: (activeIndex: any) => void;
  activeTab: number
}

const ProfileContent: React.FC<IProps> = ({ setActiveTab, activeTab }) => {
  return (
    <Tab
      menu={{ fluid: true, vertical: true }}
      menuPosition="right"
      panes={panes}
      onTabChange={(e, data) => setActiveTab(data.activeIndex)}
      activeIndex ={activeTab}
    ></Tab>
  );
};

export default observer(ProfileContent);
