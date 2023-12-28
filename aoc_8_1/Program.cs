/*
Strategy

1- Parse the input to get the instructions and keep all the nodes in memory
2- Iterate the nodes to create a linked list
3- Use the instructions to go through the linked list
*/

const char LEFT = 'L';
const string START_NODE_LABEL = "AAA";
const string END_NODE_LABEL = "ZZZ";

var input = "";
using (StreamReader sr = new("./input.txt")) {
    input = sr.ReadToEnd();
}
(string instructionList, List<(string, string, string)> rawNodeList) = parseInput(input);
List<Node> nodeList = createNodeList(rawNodeList);
var stepCount = followInstructions(instructionList, nodeList);
Console.WriteLine($"Number of steps: {stepCount}");


(string instructionList, List<(string, string, string)> rawNodeList) parseInput(string input) {
    var instructionList = "";
    List<(string, string, string)> rawNodeList = [];
    string[] lines =  input.Split('\n');
    for (var i = 0; i < lines.Length; i++) {
        if (i == 0) {
            instructionList = lines[i].Trim();
            continue;
        }
        if (string.IsNullOrEmpty(lines[i].Trim())) {
            continue;
        }
        rawNodeList.Add((lines[i].Substring(0, 3), lines[i].Substring(7, 3), lines[i].Substring(12, 3)));
    }
    return (instructionList, rawNodeList);
}

List<Node> createNodeList(List<(string, string, string)> rawNodeList) {
    List<Node> nodeList = [];
    foreach ((string label, string _, string _) in rawNodeList) {
        nodeList.Add(new(label));
    }
    for (var i = 0; i < nodeList.Count; i++) {
        Node sourceNode = nodeList[i];
        (string destinationLabel, string left, string right) = rawNodeList[i];
        sourceNode.Left = nodeList.Where(sourceNode => sourceNode.Label != destinationLabel).FirstOrDefault(sourceNode => sourceNode.Label == left);
        sourceNode.Right = nodeList.Where(sourceNode => sourceNode.Label != destinationLabel).FirstOrDefault(sourceNode => sourceNode.Label == right);
    }
    return nodeList;
}

long followInstructions(string instructionList, List<Node> nodeList) {
    long stepCount = 0;
    Node? currentNode = nodeList.Find(node => node.Label == START_NODE_LABEL);
    for(var i = 0; currentNode?.Label != END_NODE_LABEL; i++) {
        if (instructionList[i] == LEFT) {
            currentNode = currentNode?.Left;
        } else {
            currentNode = currentNode?.Right;
        }
        stepCount++;
        if (currentNode?.Label == END_NODE_LABEL) {
            break;
        }
        if (i == instructionList.Length - 1) {
            i = -1;
        }
    }
    return stepCount;
}

record Node(string Label) {
    public string Label { get; init; } = Label;
    public Node? Left { get; set; }
    public Node? Right { get; set; }
}