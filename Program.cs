Console.Clear();
System.Console.WriteLine($"The result of your formula is: {Evaluate(GetFormula())}");

string GetFormula() //get input of user
{
    System.Console.Write("Please enter a formula: ");
    return Console.ReadLine()!.Replace(" ", "");
}
int Evaluate(string formula)//Evaluate the result of the formula
{
    if (formula.Length == 0) { return 0; } //if the input is empty return 0
    formula = EvaluateCling(formula);
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
            return Evaluate(formula.Substring(0, lastIndexOfMulDiv)) * int.Parse(formula.Substring(lastIndexOfMulDiv + 1));
        }
        else
        {
            return Evaluate(formula.Substring(0, lastIndexOfMulDiv)) / int.Parse(formula.Substring(lastIndexOfMulDiv + 1));
        }
    }
    else { return int.Parse(formula); } //if there is no operator just return the number
}
//I was bored so i did clings as well so know you can use () in your formula
string EvaluateCling(string formula)//gives back a formula without clings
{
    int indexClingOpen = formula.IndexOf('(');//gets first opened cling
    int indexClingClosed = FindIndexOfClosingCling(formula);//gets the index of the fitting closing cling
    if (indexClingClosed != -1)//checks if there are still clings left in the formula
    {
        string stringToEvaluate = formula.Substring(indexClingOpen + 1, indexClingClosed - indexClingOpen - 1);
        return $"{formula.Substring(0, indexClingOpen)}{Evaluate(stringToEvaluate)}{EvaluateCling(formula.Substring(indexClingClosed + 1))}";//makes a new formula without clings
    }
    return formula;
}
int FindIndexOfClosingCling(string formula)//gives back the index of the closing cling
{
    int clingsOpened = 0;
    int clingsClosed = 0;
    for (int i = 0; i < formula.Length; i++)
    {
        if (formula[i] == '(') { clingsOpened++; }
        else if (formula[i] == ')') { clingsClosed++; }
        if (clingsOpened == clingsClosed && clingsOpened != 0) { return i; }
    }
    return -1;
}