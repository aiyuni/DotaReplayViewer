import React, { Component } from 'react';

export class Dota extends React.Component {
    static displayName = "hello dota"

    constructor(props) {
        super(props);

        this.state = {
            matchId: 0, players: null
        }

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.getMatchDetails = this.getMatchDetails.bind(this);
        this.handleMatchIdChange = this.handleMatchIdChange.bind(this);
        this.onSelectHero = this.onSelectHero.bind(this);
        this.render = this.render.bind(this);
    }

    getMatchDetails(event) {
        event.preventDefault();

        fetch('api/Dota/GetMatchDetails/' + this.state.matchId)
            .then(response => response.json())
            .then(result => {
                console.log(result);
                this.setState({ players: result.players });
            });
    }

    handleMatchIdChange(event) {
        console.log("match id was changed: " + event.target.value);
        this.setState({ matchId: event.target.value });
    }

    onSelectHero(playerSlot) {
        if (playerSlot >= 128) playerSlot -= 122;
        console.log("player slot is: " + playerSlot);

        fetch('api/Dota/StartReplay/' + this.state.matchId + "/" + playerSlot)
            .then(response => {
                console.log("inside StartReplay fetch");
            })
    }

    render() {
        return (
            <div>
                <h1>Dota</h1>

                <form onSubmit={this.getMatchDetails}>
                    <label>
                        Match Id:
                            <input type="text" value={this.state.value} onChange={this.handleMatchIdChange} />
                    </label>
                    <input type="submit" value="Submit" />
                </form>

                {this.state.players &&
                    <table className="table table-stripled">
                        <thead>
                            <tr>
                                <th>Hero Name</th>
                                <th>Player Slot</th>
                            </tr>
                        </thead>
                        <tbody>
                            {this.state.players.map((p, i) =>
                                <tr onClick={() => this.onSelectHero(p.player_slot)} key={i}>
                                    <td>{p.hero.localized_name}</td>
                                    <td>{p.player_slot}</td>
                                </tr>
                            )}
                        </tbody>
                    </table>
                }

            </div>
        );
    }
}
