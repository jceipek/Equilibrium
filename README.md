Equilibrium
===========

A prototype for a game about networks


Just open `equilibrium_game.html` with a modern browser after downloading (ideally the latest Chrome, since that's the only thing I tested it in).

The objective is to connect all the nodes (in the prototype, this is done by clicking and dragging). Note that disconnecting nodes with no other side-effects isn't possible in the interface right now -- you'll have to refresh :/
You can access debug mode with 'd' and mess around by adding, deleting, and resizing nodes.

The test data in the prototype at the moment demonstrates that the order in which connections are made is important in many cases.

The prototype is set up like a pretty static puzzle game at the moment, but that's just because I haven't tried out other things yet, such as:
1. There could be moveable nodes that can be dragged around with connections using spring physics. Then the node structure could play a huge role in the final solution. Imagine dragging a chain of nodes around to reach further.
2. Nodes could have different colors; the colors could limit which nodes could connect to which other nodes. For example, a red node might connect to red and yellow nodes and a green node might connect to green and yellow nodes. Connecting the full network would then be more challenging.
3. At the moment, the fact that mass travels along connections is implicit. If this becomes a real game, I would like to show the mass moving along connections like dew on a spider's web. As a variation on the existing mechanics, the specific "stuff" traveling between nodes might vary; imagine a game variant in which certain nodes must be disconnected to avoid destroying other nodes. This game could quickly become frantic as the player reconfigures the network in order to save it.

Where does the Rift come in?
The current prototype is 2d for the sake of rapid iteration on game ideas. While this might make for a fun diversion on a tablet, the ideal version is a full 3d network that the player can investigate by looking at it with the Rift. The ideal control scheme involves the Razer Hydra -- imagine physically "grabbing" the network to rotate it and physically moving your hands together and apart to draw a new connection between nodes.
