import React, { Component } from 'react';

export class Dota extends React.Component {
    static displayName = "hello dota"

    constructor(props) {
        super(props);
        this.state = {
            matchId: 123, heroesArray: [{ "heroName": "antimage", "playerSlot": "1" }, {
                "heroName": "bane", "playerSlot": "2"
            }], obtainedMatchData: false
        }
        
        //this.incrementCounter = this.incrementCounter.bind(this);

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.getMatchDetails = this.getMatchDetails.bind(this);
        this.handleMatchIdChange = this.handleMatchIdChange.bind(this);
        this.onSelectHero = this.onSelectHero.bind(this);
        this.render = this.render.bind(this);
    }

    //incrementCounter() {
    //    this.setState({
    //        currentCount: this.state.currentCount + 1
    //    });
    //}

     getMatchDetails(event) {

         this.setState({ obtainedMatchData: false });
         console.log("inside getMatchDetails");
        //let response = await fetch('api/Dota/GetMatchDetails/5165102419');
        //let result = await response.json();
        //console.log("test: " + Object.keys(result));
        //this.setState({ heroNames: Object.keys(result) });


         fetch('api/Dota/GetMatchDetails/' + this.state.matchId)
            .then(response => {
                console.log("inside first fetch");
                //this.setState({ heroNames: Object.keys(response.json()) });
                response.json()
                    .then(result => {
                        console.log(result);
                        console.log("hero Name result is: " + result[0]["heroName"])
                        console.log("test: " + Object.entries(result));
                        this.setState({ heroesArray: result });
                        this.setState({ obtainedMatchData: true });
                    })
            })
        
        event.preventDefault();
    }

    handleChange(event) {
        console.log("match id was changed: " + this.state.matchId);
        //this.setState({matchId: event.target.value });
    }

    handleSubmit(event) {
        //console.log("match id was submitted: " + this.matchId);
        event.preventDefault();
    }

    handleMatchIdChange(event) {
        console.log("match id was changed: " + this.state.matchId);
        this.setState({matchId: event.target.value });
    }

    onSelectHero(playerSlot) {
        console.log("player slot is: " + playerSlot);
        if (playerSlot >= 128) {
            playerSlot -= 122;
        }

        fetch('api/Dota/StartReplay/' + this.state.matchId + "/" + playerSlot)
            .then(response => {
                console.log("inside StartReplay fetch");
            })
    }

    render() {
        return (
            <div>
                <h1>Dota</h1>

                <p>This is a simple example of a React component.</p>

                <form onSubmit={this.getMatchDetails}>
                    <label>
                        Match Id:
                        <input type="text" value={this.state.value} onChange={this.handleMatchIdChange} />
                    </label>
                    <input type="submit" value="Submit" />
                </form>

                {this.state.obtainedMatchData &&
                    <table className="table table-stripled">
                        <thead>
                            <tr>
                                <th>Hero Name</th>
                                <th>Player Slot</th>
                            </tr>
                        </thead>
                        <tbody>
                            {this.state.heroesArray.map((x, i) =>
                                <tr onClick={() => this.onSelectHero(x.playerSlot)} key={i}>
                                    <td>{x.heroName}</td>
                                    <td>{x.playerSlot}</td>
                                </tr>

                            )}
                        </tbody>
                    </table>
                }

            </div>
        );
    }
}
