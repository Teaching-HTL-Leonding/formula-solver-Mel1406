Console.Clear();
System.Console.WriteLine($"The result of your formula is: {Math.Round(Evaluate(GetFormula()), 2)}");

string GetFormula() //get input of user
{
    System.Console.Write("Please enter a formula: ");
    return Console.ReadLine()!.Replace(" ", "");
}
double Evaluate(string formula)//Evaluate the result of the formula
{
    if (formula.Length == 0) { return 0; } //if the input is empty return 0
    formula = EvaluateBracket(formula);
    string subFormula = formula.Substring(1);
    if (subFormula.Contains('+') || subFormula.Contains('-')) //ignore the first character incase it is a operation
    {
        int indexOfPlus = subFormula.IndexOf('+'); //get everything except the first character
        int indexOfMinus = subFormula.IndexOf('-');
        int firstIndexOfPlusMinus = Math.Min(indexOfPlus + 1, indexOfMinus + 1); //get the minimum index of the first valid operation
        if (indexOfPlus < 0) { firstIndexOfPlusMinus = indexOfMinus + 1; } //if there is no addition or subtraction ignore the -1 of the index
        else if (indexOfMinus < 0) { firstIndexOfPlusMinus = indexOfPlus + 1; }
        return Evaluate(formula.Substring(0, firstIndexOfPlusMinus)) + Evaluate(formula.Substring(firstIndexOfPlusMinus)); //take the first number and then add the evaluated rest of the formula
    }
    else if (formula.Contains('*') || formula.Contains('/'))
    {   //with multiplications or divisions we go from the back of the formula incase there are multiple divisions or multiplications
        int lastIndexMultiply = formula.LastIndexOf('*');
        int lastIndexDivide = formula.LastIndexOf('/');
        int lastIndexOfMulDiv = Math.Max(lastIndexMultiply, lastIndexDivide);
        //depending on the operator do the operation with recursive programming
        if (lastIndexOfMulDiv == lastIndexMultiply)
        {
            return Evaluate(formula.Substring(0, lastIndexOfMulDiv)) * double.Parse(formula.Substring(lastIndexOfMulDiv + 1));
        }
        else
        {
            return Evaluate(formula.Substring(0, lastIndexOfMulDiv)) / double.Parse(formula.Substring(lastIndexOfMulDiv + 1));
        }
    }
    else { return double.Parse(formula); } //if there is no operator just return the number
}
//I was bored so i did brackets as well so know you can use () in your formula
string EvaluateBracket(string formula)//gives back a formula without brackets
{
    int indexBracketOpen = formula.IndexOf('(');//gets first opened brackets
    int indexBracketClosed = FindIndexOfClosedBracket(formula);//gets the index of the fitting closing bracket
    if (indexBracketClosed != -1)//checks if there are still clings left in the formula
    {
        string stringToEvaluate = formula.Substring(indexBracketOpen + 1, indexBracketClosed - indexBracketOpen - 1);
        return $"{formula.Substring(0, indexBracketOpen)}{Evaluate(stringToEvaluate)}{EvaluateBracket(formula.Substring(indexBracketClosed + 1))}";//makes a new formula without clings
    }
    return formula;
}
int FindIndexOfClosedBracket(string formula)//gives back the index of the closing bracket
{
    int bracketsOpened = 0;
    int bracketsClosed = 0;
    for (int i = 0; i < formula.Length; i++)
    {
        if (formula[i] == '(') { bracketsOpened++; }
        else if (formula[i] == ')') { bracketsClosed++; }
        if (bracketsOpened == bracketsClosed && bracketsOpened != 0) { return i; }
    }
    return -1;
}