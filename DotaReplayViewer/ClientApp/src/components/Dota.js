import React, { useState } from 'react';
import { Button } from '@material-ui/core';

export default function Dota() {
    const [matchId, setMatchId] = useState(0);
    const [players, setPlayers] = useState(null);

    const getMatchDetails = event => {
        event.preventDefault();

        fetch('api/Dota/GetMatchDetails/' + this.state.matchId)
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

        fetch('api/Dota/StartReplay/' + this.state.matchId + "/" + playerSlot)
            .then(response => {
                console.log("inside StartReplay fetch");
            })
    }

    return (
        <React.Fragment>
            <Button variant="contained" color="primary">
                Hello World
            </Button>
            <h1>Dota</h1>

            <form onSubmit={getMatchDetails}>
                <label>
                    Match Id:
                    <input type="text" value={matchId} onChange={handleMatchIdChange} />
                </label>
                <input type="submit" value="Submit" />
            </form>

            {players &&
                <table className="table table-stripled">
                    <thead>
                        <tr>
                            <th>Hero Name</th>
                            <th>Player Slot</th>
                        </tr>
                    </thead>
                    <tbody>
                        {players.map((p, i) =>
                            <tr onClick={() => onSelectHero(p.player_slot)} key={i}>
                                <td>
                                    <img src={`api/Dota/GetHeroImage/${p.hero.id}`} width="100px"/>
                                    {p.hero.localized_name}
                                </td>
                                <td>{p.player_slot}</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            }
        </React.Fragment>
    );
}
