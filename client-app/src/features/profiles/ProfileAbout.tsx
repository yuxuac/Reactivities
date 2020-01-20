import React, { useContext, useState } from "react";
import { Tab, Grid, Header, Button } from "semantic-ui-react";
import { RootStoreContext } from "../../app/stores/RootStore";
import { observer } from "mobx-react-lite";
import ProfileEditForm from "./ProfileEditForm";
import { IProfile } from "../../app/Models/profile";

const ProfileAbout = () => {
  const rootStore = useContext(RootStoreContext);
  const {
    profile,
    isCurrentUser,
    loading,
    editProfile
  } = rootStore.profileStore;

  const [editMode, setEditMode] = useState(false);

  const handleSubmit = async (values: Partial<IProfile>) => {
    await editProfile(values).then(() => {
      setEditMode(false);
    });
  };

  return (
    <Tab.Pane>
      <Grid>
        <Grid.Column width={16} style={{ paddingBottom: 0 }}>
          <Header
            floated="left"
            icon="user"
            content={"About " + profile?.displayName}
          ></Header>
          {isCurrentUser && (
            <Button
              floated="right"
              basic
              content={editMode ? "Cancel" : "Edit Profile"}
              onClick={e => setEditMode(!editMode)}
              disabled={editMode && loading}
            />
          )}
        </Grid.Column>
        <Grid.Column width={16}>
          {!editMode ? (
            <span>{profile?.bio}</span>
          ) : (
            <ProfileEditForm updateProfile={handleSubmit} profile={profile!} />
          )}
        </Grid.Column>
      </Grid>
    </Tab.Pane>
  );
};

export default observer(ProfileAbout);
