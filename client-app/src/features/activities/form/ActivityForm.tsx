import React, { useState, useContext, useEffect } from "react";
import { Segment, Form, Button, Grid } from "semantic-ui-react";
import {
  ActivityFormValues
} from "../../../app/Models/activity";
import { v4 as uuid } from "uuid";
import { observer } from "mobx-react-lite";
import { RouteComponentProps } from "react-router-dom";
import { Form as FinalForm, Field } from "react-final-form";
import TextInput from "../../../app/common/form/TextInput";
import TextAreaInput from "../../../app/common/form/TextAreaInput";
import SelectInput from "../../../app/common/form/SelectInput";
import { category } from "../../../app/common/options/categoryOptions";
import DateInput from "../../../app/common/form/DateInput";
import { combineDateAndTime } from "../../../app/common/util/util";
import { runInAction } from "mobx";
import {
  combineValidators,
  isRequired,
  composeValidators,
  hasLengthGreaterThan
} from "revalidate";
import { RootStoreContext } from "../../../app/stores/RootStore";

const validate = combineValidators({
  title: isRequired({ message: "The event title is required" }),
  category: isRequired("Category"),
  description: composeValidators(
    isRequired("Description"),
    hasLengthGreaterThan(4)({
      message: "Description needs to be at least 5 characters."
    })
  )(),
  city: isRequired("City"),
  venue: isRequired("Venue"),
  date: isRequired("Date"),
  time: isRequired("Time")
});

interface DetailParams {
  id: string;
}

const ActivityForm: React.FC<RouteComponentProps<DetailParams>> = ({
  match,
  history
}) => {
  const rootStore = useContext(RootStoreContext);
  const {
    createActivity,
    editActivity,
    submitting,
    loadActivity
  } = rootStore.activityStore;

  // Input: initializeForm, output: IActivity
  const [activity, setActivity] = useState(new ActivityFormValues());
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (match.params.id) {
      setLoading(true);
      loadActivity(match.params.id)
        .then(activity =>
          runInAction("initializing...", () => {
            setActivity(new ActivityFormValues(activity));
          })
        )
        .finally(() =>
          runInAction("initializing...", () => {
            setLoading(false);
          })
        );
    }
  }, [loadActivity, match.params.id]);

  const handleFinalFormSubmit = (values: any) => {
    const dateAndTime = combineDateAndTime(values.date, values.time);
    const { date, time, ...activity } = values;
    activity.date = dateAndTime;
    console.log(activity);
    if (!activity.id) {
      let newActivity = {
        ...activity,
        id: uuid()
      };
      createActivity(newActivity);
    } else {
      editActivity(activity);
    }
  };

  return (
    <Grid>
      <Grid.Column width={10}>
        <Segment clearing>
          <FinalForm
            validate={validate}
            initialValues={activity}
            onSubmit={handleFinalFormSubmit}
            render={({ handleSubmit, invalid, pristine }) => (
              <Form onSubmit={handleSubmit} loading={loading}>
                <Field
                  placeholder="Title"
                  name="title"
                  value={activity.title}
                  component={TextInput}
                />
                <Field
                  name="description"
                  placeholder="Description"
                  value={activity.description}
                  rows={3}
                  component={TextAreaInput}
                />
                <Field
                  placeholder="Category"
                  options={category}
                  name="category"
                  value={activity.category}
                  component={SelectInput}
                />
                <Form.Group widths="equal">
                  <Field
                    placeholder="Date"
                    name="date"
                    date={true}
                    value={activity.date}
                    component={DateInput}
                  />
                  <Field
                    placeholder="Time"
                    name="time"
                    time={true}
                    value={activity.date}
                    component={DateInput}
                  />
                </Form.Group>

                <Field
                  placeholder="City"
                  name="city"
                  value={activity.city}
                  component={TextInput}
                />
                <Field
                  component={TextInput}
                  placeholder="Venue"
                  name="venue"
                  value={activity.venue}
                />

                <Button
                  loading={submitting}
                  floated="right"
                  positive
                  type="submit"
                  content="Submit"
                  disabled={loading || invalid || pristine}
                />
                <Button
                  onClick={
                    activity.id
                      ? () => history.push(`/activities/${activity.id}`)
                      : () => history.push("/activities")
                  }
                  floated="right"
                  type="button"
                  content="Cancel"
                  disabled={loading}
                />
              </Form>
            )}
          />
        </Segment>
      </Grid.Column>
    </Grid>
  );
};

export default observer(ActivityForm);
