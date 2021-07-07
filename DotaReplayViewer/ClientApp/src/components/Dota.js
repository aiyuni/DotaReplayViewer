import React, { useState, useEffect } from 'react';
import { Button, Typography, TextField, Table, TableContainer, Paper, TableHead, TableBody, TableRow, TableCell, Box, Container } from '@material-ui/core';
import { makeStyles, withStyles } from '@material-ui/core/styles';

const useStyles = makeStyles(theme => ({
    formContainer: {
        textAlign: 'center',
        padding: theme.spacing(4, 8)
    },
    heroImage: {
        marginRight: theme.spacing(2)
    },
    playersTable: {
        marginBottom: theme.spacing(4)
    },
    submit: {
        margin: theme.spacing(1, 0, 0)
    }
}));

const CustomTextField = withStyles(theme => ({
    root: {
        '& label.Mui-focused': {
            color: theme.palette.text.primary
        }
    }
}))(TextField);

export default function Dota() {
    const [matchId, setMatchId] = useState('');
    const [matchDetails, setMatchDetails] = useState(null);
    useEffect(() => {
        console.log(matchId);
        getMatchDetails(matchId);
    }, [matchId]); 
    
    const classes = useStyles();

    const getMatchDetails = async matchId => {
        const response = await fetch(`api/Dota/GetMatchDetails/${matchId}`);
        const data = response.status === 200
            ? await response.json()
            : null;
        console.log('data', data);
        setMatchDetails(data);
    }

    const handleMatchIdChange = event => {
        const matchId = event.target.value;
        setMatchId(matchId);
    }

    const handleSelectHero = async playerSlot => {
        if (playerSlot >= 128) playerSlot -= 122;
        await fetch(`api/Dota/StartReplay/${matchDetails.matchId}/${playerSlot}`);
    }

    return (
        <React.Fragment>
            <Container className={classes.formContainer} maxWidth="xs">
                <Typography variant="h5" gutterBottom>Enter Match ID</Typography>
                <form>
                    <CustomTextField
                        inputProps={{ style: { textAlign: 'center' } }}
                        fullWidth
                        required
                        id="matchId"
                        autoFocus
                        value={matchId}
                        onChange={handleMatchIdChange}
                    />
                </form>
            </Container>
            {matchDetails &&
                <TableContainer className={classes.playersTable} component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>Hero Name</TableCell>
                                <TableCell>Player Slot</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {matchDetails.players.map((p, i) =>
                                <TableRow onClick={() => handleSelectHero(p.player_slot)} key={i}>
                                    <TableCell>
                                        <Box display="flex" flexDirection="row" alignItems="center">
                                            <img className={classes.heroImage} src={`api/Dota/GetHeroImage/${p.hero.id}`} width="100px" />
                                            <Typography variant="body1">{p.hero.localized_name}</Typography>
                                        </Box>
                                    </TableCell>
                                    <TableCell>{p.player_slot}</TableCell>
                                </TableRow>
                            )}
                        </TableBody>
                    </Table>
                </TableContainer>
            }
        </React.Fragment>
    );
}
