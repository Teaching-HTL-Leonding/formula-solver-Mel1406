#region main program
Console.Clear();
System.Console.WriteLine($"The result of your formula is: {Math.Round(Evaluate(GetFormula()), 2)}");
#endregion

#region methods
//get input of user
string GetFormula()
{
    System.Console.Write("Please enter a formula: ");
    return Console.ReadLine()!.Replace(" ", "");
}

//Evaluate the result of the formula
double Evaluate(string formula)
{
    //if the input is empty return 0
    if (formula.Length == 0) { return 0; }
    //simplifies the formula so there are no brackets anymore
    formula = EvaluateBracket(formula);

    string subFormula = formula.Substring(1);

    //ignore the first character incase it is a operator like - so we can add negative numbers to get the effect of a subtraction
    if (subFormula.Contains('+') || subFormula.Contains('-'))
    {
        //checks for the first + or - in the formula but ignores the first character
        int indexOfPlus = subFormula.IndexOf('+');
        int indexOfMinus = subFormula.IndexOf('-');

        //get the minimum index of the first valid operation
        int firstIndexOfPlusMinus = Math.Min(indexOfPlus + 1, indexOfMinus + 1);

        //if there is no + or no - the methods IndexOf returns -1 which is smaller than 0 but there is no real index -1
        //-1 gets ignored and the other operation index becomes the index of the first valid operation
        if (indexOfPlus < 0) { firstIndexOfPlusMinus = indexOfMinus + 1; }
        else if (indexOfMinus < 0) { firstIndexOfPlusMinus = indexOfPlus + 1; }

        //take the first part of the formula before the index and run it through the method again to check if there are any / or *
        //do the same process for the part behind the operation
        //all the additions get stacked to be added together and then returned if there are no operations left
        return Evaluate(formula.Substring(0, firstIndexOfPlusMinus)) + Evaluate(formula.Substring(firstIndexOfPlusMinus));
    }
    else if (formula.Contains('*') || formula.Contains('/'))
    {
        //with multiplications or divisions we go from the back of the formula incase there are multiple divisions or multiplications because we always have to do the ones on the left first
        //get the last * or / operation in the current part 
        int lastIndexMultiply = formula.LastIndexOf('*');
        int lastIndexDivide = formula.LastIndexOf('/');
        int lastIndexOfMulDiv = Math.Max(lastIndexMultiply, lastIndexDivide);

        //depending on the operator take the first half and let it go through the method again and then do the fitting operation
        if (lastIndexOfMulDiv == lastIndexMultiply)
        {
            return Evaluate(formula.Substring(0, lastIndexOfMulDiv)) * double.Parse(formula.Substring(lastIndexOfMulDiv + 1));
        }
        else
        {
            return Evaluate(formula.Substring(0, lastIndexOfMulDiv)) / double.Parse(formula.Substring(lastIndexOfMulDiv + 1));
        }
    }
    //if there is no operator just return the number
    else { return double.Parse(formula); }
}

//gives back a formula without brackets
string EvaluateBracket(string formula)
{
    //gets the index of the first opened bracket
    int indexBracketOpen = formula.IndexOf('(');
    //gets the index of the fitting closing bracket
    int indexBracketClosed = FindIndexOfClosedBracket(formula);
    //checks if there are still brackets left in the formula
    //if yes, take the part before the the bracket, evaluate the result of the content in the current bracket and check the rest after the closed bracket for other brackets
    if (indexBracketClosed != -1)
    {
        string stringToEvaluate = formula.Substring(indexBracketOpen + 1, indexBracketClosed - indexBracketOpen - 1);
        //returns the new formula without any brackets
        return $"{formula.Substring(0, indexBracketOpen)}{Evaluate(stringToEvaluate)}{EvaluateBracket(formula.Substring(indexBracketClosed + 1))}";
    }
    //if there are no more brackets just return the part of the formula
    return formula;
}

//gives back the index of the closing bracket
int FindIndexOfClosedBracket(string formula)
{
    int bracketsOpened = 0;
    int bracketsClosed = 0;
    //every bracket that gets opened has to be closed 
    //if there is another bracket in the bracket, only the second closing bracket marks the end of the content to be currently evaluated instead of the first one
    for (int i = 0; i < formula.Length; i++)
    {
        if (formula[i] == '(') { bracketsOpened++; }
        else if (formula[i] == ')') { bracketsClosed++; }
        if (bracketsOpened == bracketsClosed && bracketsOpened != 0) { return i; }
    }
    //if there are no brackets left return -1
    return -1;
}
#endregion