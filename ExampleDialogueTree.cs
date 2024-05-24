using Godot;
using System;
using System.Collections.Generic;

public partial class ExampleDialogueTree : Node
{
    private DialogueTree dialogueTree;
    [Export] int age = 10;
    bool isOld => age >= 18;
    bool isVeryOld => age >= 50;

	public ExampleDialogueTree() {
        BaseDialogueNode startNode = new ConditionNode.Builder()
            .SetCondition(() => isOld)
            .SetTrueNode(new ConditionNode.Builder()
                .SetCondition(() => isVeryOld)
                .SetTrueNode(new DialogueNode("You're very old."))
                .SetFalseNode(new DialogueNode("You're old but not that old"))
                .Build()
            )
            .SetFalseNode(new DialogueNode(new List<string>() { "You're not old"}))
            .Build();
        dialogueTree = new DialogueTree(startNode);
    }

    public override void _Ready() {
        dialogueTree.OnDialogueSent += HandleDialogue;
        dialogueTree.OnQuestionSent += HandleQuestion;
        dialogueTree.Evaluate();
    }

    private void HandleQuestion(object sender, (string, List<string>) e) {
        GD.Print("Question: " + e.Item1);
        GD.Print("Answers: ");
        for (int i = 0; i < e.Item2.Count; i++) {
            GD.Print(i + ". " + e.Item2[i]);
        }
    }

    private void HandleDialogue(object sender, List<string> e) {
        for (int i = 0; i < e.Count; i++) {
            GD.Print(i + ". " + e[i]);
        }
    }
}
