import React, { useContext, useEffect } from "react";
import { Grid } from "semantic-ui-react";
import ActivityList from "./ActivityList";
import { observer } from 'mobx-react-lite'
import { useLocation } from "react-router-dom";
import { LoadingComponent } from "../../../app/layout/LoadingComponent";
import ActivityStore from "../../../app/stores/ActivityStore";

const ActivityDashboard: React.FC = () => {
  const activityScore = useContext(ActivityStore);
  const { pathname } = useLocation();
  
  useEffect(() => {
    activityScore.loadActivities();
  }, [activityScore]);

  useEffect(() => {
    window.scrollTo(0, 0);
  }, [pathname]);
  
  if (activityScore.loadingInitial)
    return <LoadingComponent content="Loading activities..." />;

  return (
    <Grid>
      <Grid.Column width={10}>
        <ActivityList></ActivityList>
      </Grid.Column>
      <Grid.Column width={6}>
        <h2>Activity filters</h2>
      </Grid.Column>
    </Grid>
  );
};

export default observer(ActivityDashboard);