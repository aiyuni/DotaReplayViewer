import React from 'react';
import { Container, AppBar, Typography, Toolbar } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles(theme => ({
    appBar: {
        marginBottom: theme.spacing(4)
    },
    toolbar: {
        padding: theme.spacing(2)
    },
    icon: {
        marginRight: theme.spacing(2)
    }
}));

export default function Layout(props) {
    const classes = useStyles();

    return (
        <div>
            <AppBar className={classes.appBar} position="static">
                <Toolbar className={classes.toolbar}>
                    <img className={classes.icon} src="/img/dota-2.svg" width="50px" />
                    <Typography variant="h6">Dota Replay Viewer</Typography>
                </Toolbar>
            </AppBar>
            <Container>
                {props.children}
            </Container>
        </div>
    );
}
