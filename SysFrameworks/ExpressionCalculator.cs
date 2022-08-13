using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SysFrameworks
{
    public class ExpressionCalculator
    {
        private static string GetNextToken(ref string congThuc)
        {
            try
            {
                Regex checker = new Regex(@"^(([a-zA-Z])|([0-9]+(\.[0-9]*)?)|\(|\)|\+|\-|\*|\/)");
                MatchCollection matchCollecltion = checker.Matches(congThuc.Trim());
                if (matchCollecltion.Count > 0)
                {
                    string retValue = matchCollecltion[0].Value;
                    congThuc = congThuc.Substring(retValue.Length).Trim();
                    return retValue;
                }
                else
                {
                    throw new Exception(string.Format("Không thể phân tích công thức {0}", congThuc));
                }
            }
            catch
            {
                throw new Exception(string.Format("Lỗi khi xử lý công thức {0}", congThuc));
            }
        }

        private static bool CheckStringIsToken(string input)
        {
            return (CheckTokenIsOperator(input) || CheckTokenIsOperand(input)
                    || input == "(" || input == ")");
        }

        private static bool CheckTokenIsOperator(string token)
        {
            return token == "+" || token == "-" || token == "*" || token == "/";
        }

        private static bool CheckTokenIsOperand(string token)
        {
            Regex checker = new Regex(@"([0-9]+(\.[0-9]*)?)");
            return checker.IsMatch(token);
        }

        private static int GetPriorityOfToken(string token)
        {
            if (token == "(" || token == ")") return 6;
            else if (token == "*" || token == "/") return 10;
            else if (token == "+" || token == "-") return 9;
            else return 7;
        }

        private static ArrayList ConvertToPolandNotation(string congThuc)
        {
            ArrayList polandNotation = new ArrayList();
            Stack stack = new Stack();
            congThuc = congThuc.Trim().ToUpper();
            Regex regexFindComment = new Regex(@"/\*.*?\*/");
            MatchCollection matchCollecltion = regexFindComment.Matches(congThuc);
            foreach (Match match in matchCollecltion)
            {
                congThuc = congThuc.Replace(match.Value, "");
            }

            while (congThuc != "")
            {
                string token = GetNextToken(ref congThuc);
                if (token == "(") stack.Push(token);
                else if (token == ")")
                {
                    string popedToken = "";
                    while (popedToken != "(" && stack.Count > 0)
                    {
                        popedToken = (string)stack.Pop();
                        if (popedToken != "(")
                            polandNotation.Add(popedToken);
                    }
                }
                else if (CheckTokenIsOperator(token))
                {
                    while (stack.Count > 0 && GetPriorityOfToken(token) <= GetPriorityOfToken((string)stack.Peek()))
                    {
                        string popedToken = (string)stack.Pop();
                        if (popedToken != "(") polandNotation.Add(popedToken);
                    }
                    stack.Push(token);
                }
                else if (CheckTokenIsOperand(token))
                {
                    polandNotation.Add(token);
                }
            }
            while (stack.Count > 0)
            {
                string popedToken = (string)stack.Pop();
                polandNotation.Add(popedToken);
            }

            return polandNotation;
        }

        private static int GetValueFromAPolandNotation(ArrayList polandNotation)
        {
            Stack stack = new Stack();
            foreach (string token in polandNotation)
            {
                if (CheckTokenIsOperand(token))
                {
                    int value = GetValueOfAOperand(token);
                    stack.Push(value);
                }
                else if (CheckTokenIsOperator(token))
                {
                    if (stack.Count < 2) throw new Exception("Công thức không hợp lệ");
                    int rightValue = (int)stack.Pop();
                    int leftValue = (int)stack.Pop();
                    switch (token)
                    {
                        case "+":
                            stack.Push(rightValue + leftValue);
                            break;
                        case "-":
                            stack.Push(leftValue - rightValue);
                            break;
                        case "*":
                            stack.Push(rightValue * leftValue);
                            break;
                        case "/":
                            if (leftValue != 0)
                                stack.Push(leftValue / rightValue);
                            else
                                throw new Exception("Công thức không thể chia cho 0");
                            break;
                    }
                }
            }
            if (stack.Count >= 1 && stack.Peek() is int) return (int)stack.Peek();
            else return 0;
        }

        private static int GetValueOfAOperand(string operand)
        {
            if (Regex.IsMatch(operand, @"[0-9]+(\.[0-9]*)?"))
            {
                return int.Parse(operand);
            }
            if (Regex.IsMatch(operand, @"[a-zA-Z]+"))
            {
                //Tạm thời fix giá trị O,N,D,X,Y,Z phục vụ panasonic
                //Khi nào có giải pháp tốt hơn thì sẽ thay thế sau
                switch (operand)
                {
                    case "O":
                    case "o":
                    case "x":
                    case "X":
                        return 10;
                    case "N":
                    case "n":
                    case "Y":
                    case "y":
                        return 11;
                    case "D":
                    case "d":
                    case "Z":
                    case "z":
                        return 12;
                    default:
                        return 0;
                }
            }
            return 0;
        }

        public static bool GetBooleanValueFromAExpression(string inputExpression)
        {
            string leftExpress = "", rightExpress = "", operatorExpress = "";

            //Regex leftRegex = new Regex(@".+(?=\>|\>=|\<|\<=|\=)");
            //MatchCollection matchCollecltion = leftRegex.Matches(inputExpression);
            //if (matchCollecltion.Count == 1) leftExpress = matchCollecltion[0].Value;

            //Regex rightRegex = new Regex(@"(?<=\>|\>=|\<|\<=|\=).+");
            //matchCollecltion = rightRegex.Matches(inputExpression);
            //if (matchCollecltion.Count == 1) rightExpress = matchCollecltion[0].Value;

            Regex operatorRegex = new Regex(@"\<\=|\>\=|\>|\<|\=");
            MatchCollection matchCollecltion = operatorRegex.Matches(inputExpression);
            if (matchCollecltion.Count == 1) operatorExpress = matchCollecltion[0].Value;

            var arrOperand = operatorRegex.Split(inputExpression);
            if (arrOperand.Length == 2)
            {
                leftExpress = arrOperand[0];
                rightExpress = arrOperand[1];
            }

            int leftValue = GetValueFromAExpression(leftExpress);
            int rightValue = GetValueFromAExpression(rightExpress);

            switch (operatorExpress)
            {
                case ">":
                    return leftValue > rightValue;
                case ">=":
                    return leftValue >= rightValue;
                case "<":
                    return leftValue < rightValue;
                case "<=":
                    return leftValue <= rightValue;
                case "=":
                    return leftValue == rightValue;
                default:
                    throw new Exception(string.Format("Biểu thức điều kiện {0} không hợp lệ", inputExpression));
            }
        }

        public static int GetValueFromAExpression(string inputExpression)
        {
            inputExpression = inputExpression.Trim().ToUpper();
            if (inputExpression.StartsWith("IF"))
            {
                string bieuThucDieuKien = inputExpression.Substring(inputExpression.IndexOf("IF") + 2);
                bieuThucDieuKien = bieuThucDieuKien.Substring(0, bieuThucDieuKien.IndexOf("THEN"));
                bool layBieuThucDung = GetBooleanValueFromAExpression(bieuThucDieuKien);

                Regex bieuThucGiaTri = null;
                if (layBieuThucDung)
                    bieuThucGiaTri = new Regex("(?<=THEN).+(?=ELSE)");
                else
                    bieuThucGiaTri = new Regex("(?<=ELSE).+");
                MatchCollection matchCollecltion = bieuThucGiaTri.Matches(inputExpression);
                if (matchCollecltion.Count == 1)
                {
                    return GetValueFromAExpression(matchCollecltion[0].Value);
                }
                else
                {
                    throw new Exception(string.Format("Biểu thức {0} không hợp lệ", inputExpression));
                }
            }
            else
            {
                ArrayList polandNotation = ConvertToPolandNotation(inputExpression);
                int retValue = GetValueFromAPolandNotation(polandNotation);
                return retValue;
            }
        }
    }
}
