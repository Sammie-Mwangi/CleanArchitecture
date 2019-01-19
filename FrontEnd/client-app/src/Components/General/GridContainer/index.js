import React from "react";
import Grid from "@material-ui/core/Grid";
import withStyles from "@material-ui/core/styles/withStyles";

// Custom Components
import { gridContainerStyle } from "./GridContainerStyle";

const GridContainer = ({ ...gridContainerProps }) => {
  const { classes, children } = gridContainerProps;

  return (
    <Grid
      container
      direction="row"
      justify="center"
      alignItems="center"
      className={classes.container}
    >
      {children}
    </Grid>
  );
};

export default withStyles(gridContainerStyle)(GridContainer);
