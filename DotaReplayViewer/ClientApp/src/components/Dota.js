import React, { useState } from 'react';
import { Button, Typography, TextField, Table, TableContainer, Paper, TableHead, TableBody, TableRow, TableCell, Box } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles(theme => ({
    form: {
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'flex-start',
        marginBottom: theme.spacing(2)
    },
    heroImage: {
        marginRight: theme.spacing(2)
    },
    playersTable: {
        marginBottom: theme.spacing(4)
    }
}));

export default function Dota() {
    const [matchId, setMatchId] = useState('');
    const [players, setPlayers] = useState(null);

    const classes = useStyles();

    const getMatchDetails = event => {
        event.preventDefault();

        fetch('api/Dota/GetMatchDetails/' + matchId)
            .then(response => response.json())
            .then(result => {
                console.log(result);
                setPlayers(result.players);
            });
    }

    const handleMatchIdChange = event => {
        console.log("match id was changed: " + event.target.value);
        setMatchId(event.target.value);
    }

    const onSelectHero = playerSlot => {
        if (playerSlot >= 128) playerSlot -= 122;
        console.log("player slot is: " + playerSlot);

        fetch('api/Dota/StartReplay/' + matchId + "/" + playerSlot)
            .then(response => {
                console.log("inside StartReplay fetch");
            })
    }

    return (
        <React.Fragment>
            <Typography variant="h6">Enter Match ID</Typography>

            <form className={classes.form} onSubmit={getMatchDetails}>
                <TextField
                    margin="normal"
                    required
                    id="matchId"
                    label="Match ID"
                    autoFocus
                    value={matchId}
                    onChange={handleMatchIdChange}
                />
                <Button
                    type="submit"
                    variant="contained"
                    color="primary"
                >
                    Search
                </Button>
            </form>

            {players &&
                <TableContainer className={classes.playersTable} component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>Hero Name</TableCell>
                                <TableCell>Player Slot</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {players.map((p, i) =>
                                <TableRow onClick={() => onSelectHero(p.player_slot)} key={i}>
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
