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
    formula = EvaluateBracket(formula);

    formula = GetRidOfDoubleDashCalculation(formula);
    //if the input is empty return 0
    if (formula.Length == 0) { return 0; }
    //simplifies the formula so there are no brackets and no double dash calculations anymore

    //ignore the first character incase it is a operator like - so we can add negative numbers to get the effect of a subtraction
    string subFormula = formula.Substring(1);

    //gets the first - or +
    int firstValidIndexOfPlusMinus = GetSmallestOperatorIndexPlusMin(subFormula, 0);

    //checks if the first - or + operation is really valid and if the + or - doesn't belong to a number for example -4
    //so if you divide or multiply, you can multiply or divide with negative numbers as well
    while (firstValidIndexOfPlusMinus != -1 && (formula[firstValidIndexOfPlusMinus - 1] is '/' or '*') && (formula[firstValidIndexOfPlusMinus] is '+' or '-'))
    {
        //you get a new substring where the + or - which isn't really valid is excluded
        subFormula = formula.Substring(firstValidIndexOfPlusMinus + 1);
        //your get the new "valid" + or - and add it to the last index of the "valid" index so you get the right index for the whole formula
        firstValidIndexOfPlusMinus = GetSmallestOperatorIndexPlusMin(subFormula, firstValidIndexOfPlusMinus);
        //example: 2*5/-4-5  return:4    | you don't want to take the first - because it belongs to the 4, go into the loop with 4-5
        //         4-5       return:2    | because you need the position of the - in the whole formula you have to add the 2 to the 4
        //index 6 would be the first valid index for a + or a - in 2*5/-4-5 
    }
    if (subFormula.Contains('+') || subFormula.Contains('-'))
    {
        string mulOrDivFormula = formula.Substring(0, firstValidIndexOfPlusMinus);
        string afterFormula = formula.Substring(firstValidIndexOfPlusMinus);
        //take the first part of the formula before the index and run it through the method again to check if there are any / or *
        //do the same process for the part behind the operation
        //all the additions get stacked to be added together and then returned if there are no operations left
        return Evaluate(mulOrDivFormula) + Evaluate(formula.Substring(firstValidIndexOfPlusMinus));
    }
    else if (formula.Contains('*') || formula.Contains('/'))
    {
        //with multiplications or divisions we go from the back of the formula incase there are multiple divisions or multiplications 
        //we always have to do the ones on the left first
        
        //get the last * or / operation in the current part 
        int lastIndexMultiply = formula.LastIndexOf('*');
        int lastIndexDivide = formula.LastIndexOf('/');
        int lastIndexOfMulDiv = Math.Max(lastIndexMultiply, lastIndexDivide);

        string firstFormula = formula.Substring(0, lastIndexOfMulDiv);
        string secondFormula = formula.Substring(lastIndexOfMulDiv + 1);
        //depending on the operator take the first half and let it go through the method again and then do the fitting operation
        if (lastIndexOfMulDiv == lastIndexMultiply)
        {
            return Evaluate(firstFormula) * double.Parse(secondFormula);
        }
        else
        {
            return Evaluate(firstFormula) / double.Parse(secondFormula);
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
        string beforeBracket = formula.Substring(0, indexBracketOpen);
        string afterBracket = formula.Substring(indexBracketClosed + 1);
        //returns the new formula without any brackets
        return $"{beforeBracket}{Evaluate(stringToEvaluate)}{EvaluateBracket(afterBracket)}";
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

int GetSmallestOperatorIndexPlusMin(string formula, int add)
{
    //checks for the first + or - in the formula but ignores the first character
    int indexOfPlus = formula.IndexOf('+');
    int indexOfMinus = formula.IndexOf('-');

    //get the minimum index of the first valid operation
    int firstIndexOfPlusMinus = Math.Min(indexOfPlus + 1 + add, indexOfMinus + 1 + add);

    //if there is no - or + return -1
    if (indexOfPlus < 0 && indexOfMinus < 0) {return -1;}
    //if there is no + or no - the methods IndexOf returns -1 which is smaller than 0 but there is no real index -1
    //-1 gets ignored and the other operation index becomes the index of the first valid operation
    if (indexOfPlus < 0) { firstIndexOfPlusMinus = indexOfMinus + 1 +add; }
    else if (indexOfMinus < 0) { firstIndexOfPlusMinus = indexOfPlus + 1 + add; }
    return firstIndexOfPlusMinus;
}

//simplify the formula so there are no double dash calculations like --. ++. +- or -+
string GetRidOfDoubleDashCalculation(string formula)
{
    //go through the formula and every time there is a double dash calculation replace it with the according operation
    for (int i = 0; i < formula.Length - 1; i++)
    {
        char firstChar = formula[i];
        char secondChar = formula[i + 1];
        if ((firstChar == '+' && secondChar == '+') || (firstChar == '-' && secondChar == '-'))
        {
            formula = $"{formula.Substring(0, i )}+{formula.Substring(i + 2)}";
            i++;
        }
        else if ((firstChar == '-' && secondChar == '+') || (firstChar == '+' && secondChar == '-'))
        {
            formula = $"{formula.Substring(0, i)}-{formula.Substring(i + 2)}";
            i++;
        }
    }
    return formula;
}
#endregion