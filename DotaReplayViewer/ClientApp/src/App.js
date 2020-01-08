import React from 'react';
import { CssBaseline, ThemeProvider } from '@material-ui/core';
import { createMuiTheme } from '@material-ui/core/styles';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Dota from './components/Dota';
import { blueGrey } from '@material-ui/core/colors';

const theme = createMuiTheme({
    palette: {
        type: 'dark',
        primary: blueGrey
    }
});

export default function App() {
    return (
        <React.Fragment>
            <ThemeProvider theme={theme}>
                <CssBaseline />
                <Layout>
                    <Route exact path='/' component={Dota} />
                </Layout>
            </ThemeProvider>
        </React.Fragment>
    );
}
