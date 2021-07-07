import React from 'react';
import { Container, AppBar, Typography, Toolbar } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles(theme => ({
    toolbar: {
        padding: theme.spacing(1, 2),
        display: 'flex',
        justifyContent: 'center'
    },
    icon: {
        marginRight: theme.spacing(1)
    }
}));

export default function Layout(props) {
    const classes = useStyles();

    return (
        <div>
            <AppBar position="static">
                <Toolbar className={classes.toolbar}>
                    <img className={classes.icon} src="/img/dota-2.svg" width="40px" />
                    <Typography variant="h1">Dota Replay Viewer</Typography>
                </Toolbar>
            </AppBar>
            <Container maxWidth="lg">
                {props.children}
            </Container>
        </div>
    );
}
