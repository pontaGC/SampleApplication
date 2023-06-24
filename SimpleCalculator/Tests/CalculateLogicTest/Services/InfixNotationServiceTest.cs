using SimpleCalculator.CalculateLogic.Core.Extensions;
using SimpleCalculator.CalculateLogic.Core.Services;
using System.Data;

namespace CalculateLogicTest.Services
{
    [TestClass]
    public class InfixNotationServiceTest
    {
        private readonly IInfixNotationService infixNotationService;

        public InfixNotationServiceTest()
        {
            var container = ContainerProvider.GetContainer();
            this.infixNotationService = container.Resolve<IInfixNotationService>();
        }

        #region ConvertInFixToRPN

        [TestMethod]
        public void ConvertInFixToRPN_ExpressionArgIsNullOrEmpty_ThrowArgumentNullException()
        {
            // Arrange

            // Act & Assert
            var nullEx = Assert.ThrowsException<ArgumentNullException>(
                () =>
                {
                    this.infixNotationService.ConvertInFixToRPN(null);
                });

            var emptyEx = Assert.ThrowsException<ArgumentNullException>(
                () =>
                {
                    this.infixNotationService.ConvertInFixToRPN(string.Empty);
                });
        }

        [TestMethod]
        public void ConvertInFixToRPN_ExistUnknownSymbol_ThrowSyntaxErrorException()
        {
            // Arrange
            const string UnknownOperator = @"10 % 3";
            const string UnknownBracket = @"1 + 2 { 2 * 3 }";

            // Act & Assert
            var unknownOperatorEx = Assert.ThrowsException<SyntaxErrorException>(
                () =>
                {
                    this.infixNotationService.ConvertInFixToRPN(UnknownOperator);
                });

            var unknownBracketEx = Assert.ThrowsException<SyntaxErrorException>(
                () =>
                {
                    this.infixNotationService.ConvertInFixToRPN(UnknownBracket);
                });
        }

        [TestMethod]
        public void ConvertInFixToRPN_InvalidRoundBracketFormat_ThrowSyntaxErrorException()
        {
            // Arrange
            const string LeftBracketOnly = "( 1 + 1 (";
            const string RightBracketOnly = ") 2 + 2 )";
            const string UncloseBracket = "1 + 2 ) + ( 2 + 3";
            const string ManyLeftBracket = "( 1 + 2 ) + ( 3 + 4";
            const string ManyRightBracket = "( 1 + 2 ) + 3 + 4 )";

            // Act & Assert
            var leftBracketOnlyEx = Assert.ThrowsException<SyntaxErrorException>(
                () =>
                {
                    this.infixNotationService.ConvertInFixToRPN(LeftBracketOnly);
                });

            var rightBracketOnlyEx = Assert.ThrowsException<SyntaxErrorException>(
                () =>
                {
                    this.infixNotationService.ConvertInFixToRPN(RightBracketOnly);
                });

            var uncloseBracketEx = Assert.ThrowsException<SyntaxErrorException>(
                () =>
                {
                    this.infixNotationService.ConvertInFixToRPN(UncloseBracket);
                });

            var manyLeftBracketEx = Assert.ThrowsException<SyntaxErrorException>(
                () =>
                {
                    this.infixNotationService.ConvertInFixToRPN(ManyLeftBracket);
                });

            var manyRightBracketEx = Assert.ThrowsException<SyntaxErrorException>(
                () =>
                {
                    this.infixNotationService.ConvertInFixToRPN(ManyRightBracket);
                });
        }

        [TestMethod]
        public void ConvertInFixToRPN_ExistUnknownIdentifier_ThrowSyntaxErrorException()
        {
            // Arrange
            const string AlphabetString = @"10 + D";
            const string MultiByteString = @"1 + ‚Q"; // 2 is multi-byte string

            // Act & Assert
            var alphabetStringEx = Assert.ThrowsException<SyntaxErrorException>(
                () =>
                {
                    this.infixNotationService.ConvertInFixToRPN(AlphabetString);
                });

            var multiByteStringEx = Assert.ThrowsException<SyntaxErrorException>(
                () =>
                {
                    this.infixNotationService.ConvertInFixToRPN(MultiByteString);
                });
        }

        [TestMethod]
        public void ConvertInFixToRPN_SingleOperator_ReturnSimpleRpnExpression()
        {
            // Arrange
            const string AddIntegersExpression = "1 + 20";
            const string AddRealNumbersExpression = "10.2 + 1.38";
            const string SubtractIntegersExpression = "100 - 33";
            const string SubtractRealNumbersExpression = "11.48 - 1.3";
            const string MultiplyIntegersExpression = "100 33 *";
            const string MultiplyRealNumbersExpression = "11.48 * 1.3";
            const string DivideIntegersExpression = "100 / 33";
            const string DivideRealNumbersExpression = "11.48 / 1.3";

            // Act
            var addIntegersActual = this.infixNotationService.ConvertInFixToRPN(AddIntegersExpression)
                                                             .ToExpressionString();
            var addRealNumbersActual = this.infixNotationService.ConvertInFixToRPN(AddRealNumbersExpression)
                                                                .ToExpressionString();
            var subtractIntegersActual = this.infixNotationService.ConvertInFixToRPN(SubtractIntegersExpression)
                                                                  .ToExpressionString();
            var subractRealNumbersActual = this.infixNotationService.ConvertInFixToRPN(SubtractRealNumbersExpression)
                                                                    .ToExpressionString();
            var multiplyIntegersActual = this.infixNotationService.ConvertInFixToRPN(MultiplyIntegersExpression)
                                                                  .ToExpressionString();
            var multiplyRealNumbersActual = this.infixNotationService.ConvertInFixToRPN(MultiplyRealNumbersExpression)
                                                      .ToExpressionString();
            var divideIntegersActual = this.infixNotationService.ConvertInFixToRPN(DivideIntegersExpression)
                                                                .ToExpressionString();
            var divideRealNumbersActual = this.infixNotationService.ConvertInFixToRPN(DivideRealNumbersExpression)
                                                                   .ToExpressionString();

            // Assert
            Assert.AreEqual("1 20 +", addIntegersActual);
            Assert.AreEqual("10.2 1.38 +", addRealNumbersActual);
            Assert.AreEqual("100 33 -", subtractIntegersActual);
            Assert.AreEqual("11.48 1.3 -", subractRealNumbersActual);
            Assert.AreEqual("100 33 *", multiplyIntegersActual);
            Assert.AreEqual("11.48 1.3 *", multiplyRealNumbersActual);
            Assert.AreEqual("100 33 /", divideIntegersActual);
            Assert.AreEqual("11.48 1.3 /", divideRealNumbersActual);
        }

        [TestMethod]
        public void ConvertInFixToRPN_MultipleOperator_ReturnRPNWithPriority()
        {
            // Arrange
            const string SamePriorityExpression1 = "50 + 100 - 30";
            const string SamePriorityExpression2 = "50 * 100 / 30";
            const string DifferentPriorityExpression1 = "50 + 100 * 30";
            const string DifferentPriorityExpression2 = "50 * 100 + 30";
            const string AllOperatorExpression = "50 * 100 + 50 / 10 - 10";

            // Act
            var samePriorityExp1 = this.infixNotationService.ConvertInFixToRPN(SamePriorityExpression1).ToExpressionString();
            var samePriorityExp2 = this.infixNotationService.ConvertInFixToRPN(SamePriorityExpression2).ToExpressionString();
            var DiffPriorityExp1 = this.infixNotationService.ConvertInFixToRPN(DifferentPriorityExpression1).ToExpressionString();
            var DiffPriorityExp2 = this.infixNotationService.ConvertInFixToRPN(DifferentPriorityExpression2).ToExpressionString();
            var allPriorityExp = this.infixNotationService.ConvertInFixToRPN(AllOperatorExpression).ToExpressionString();

            // Assert
            Assert.AreEqual("50 100 + 30 -", samePriorityExp1);
            Assert.AreEqual("50 100 * 30 /", samePriorityExp2);
            Assert.AreEqual("50 100 30 * +", DiffPriorityExp1);
            Assert.AreEqual("50 100 * 30 +", DiffPriorityExp2);
            Assert.AreEqual("50 100 * 50 10 / + 10 -", allPriorityExp);
        }

        [TestMethod]
        public void ConvertInFixToRPN_ContainsRoundBracket_ReturnRPNWithPriority()
        {
            // Arrange
            const string StartBracketExpression = "( 50 +  100 ) * 30";
            const string IntermidiateBracketExpression = "50 * ( 100 + 30 ) - 10";
            const string EndBracketExpression = "( 50 +  100 ) / 30";
            const string MultiBracketExpression = "( 10 + 20 ) * ( 110 - 10 )";

            // Act
            var startBracketExp = this.infixNotationService.ConvertInFixToRPN(StartBracketExpression).ToExpressionString();
            var intermidiateBracketExp = this.infixNotationService.ConvertInFixToRPN(IntermidiateBracketExpression).ToExpressionString();
            var endBracketExpression = this.infixNotationService.ConvertInFixToRPN(EndBracketExpression).ToExpressionString();
            var multiBracketExp = this.infixNotationService.ConvertInFixToRPN(MultiBracketExpression).ToExpressionString();

            // Assert
            Assert.AreEqual("50 100 + 30 *", startBracketExp);
            Assert.AreEqual("50 100 30 + * 10 -", intermidiateBracketExp);
            Assert.AreEqual("50 100 + 30 /", endBracketExpression);
            Assert.AreEqual("10 20 + 110 10 - *", multiBracketExp);
        }

        [TestMethod]
        public void ConvertInFixToRPN_ContainsNestedRoundBracket_ReturnRPNWithPriority()
        {
            // Arrange
            const string SequentialOpenBracketExpression = "( ( 50 +  100 ) * 60 ) * 30";
            const string IntermidiateBracketExpression = "( 50 * ( 100 + 30 ) / 10 ) / 10";
            const string SequentialCloseBracketExpression = "( 10 * ( 50 +  100 ) ) * 20";
            const string MultipleNestBracketExpression = "3 / ( ( ( 6 + 3 ) / 3 ) * 10 )";

            // Act
            var sequntialOpenBracketExp = this.infixNotationService.ConvertInFixToRPN(SequentialOpenBracketExpression).ToExpressionString();
            var intermidiateBracketExp = this.infixNotationService.ConvertInFixToRPN(IntermidiateBracketExpression).ToExpressionString();
            var sequentialCloseBracketExp = this.infixNotationService.ConvertInFixToRPN(SequentialCloseBracketExpression).ToExpressionString();
            var multipleNestExpression = this.infixNotationService.ConvertInFixToRPN(MultipleNestBracketExpression).ToExpressionString();

            // Assert
            Assert.AreEqual("50 100 + 60 * 30 *", sequntialOpenBracketExp);
            Assert.AreEqual("50 100 30 + * 10 / 10 /", intermidiateBracketExp);
            Assert.AreEqual("10 50 100 + * 20 *", sequentialCloseBracketExp);
            Assert.AreEqual("3 6 3 + 3 / 10 * /", multipleNestExpression);
        }

        #endregion

        #region Calcuate

        [TestMethod]
        public void Calculate_ExpressionIsNullOrEmtpy_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => this.infixNotationService.Calculate(infixExpression:null));
            Assert.ThrowsException<ArgumentNullException>(() => this.infixNotationService.Calculate(string.Empty));
        }

        [TestMethod]
        public void Calculate_ExpressionContainsUnknownOperator_ThrowArithmeticException()
        {
            // Arrange
            const string InputExpression = "1 | 3";

            // Act & Assert
            Assert.ThrowsException<ArithmeticException>(() => this.infixNotationService.Calculate(InputExpression));
        }

        [TestMethod]
        public void Calculate_ExistUnknownIdentifier_ThrowArithmeticException()
        {
            // Arrange
            const string AlphabetString = @"10 + D";
            const string MultiByteString = @"1 + ‚Q"; // 2 is multi-byte string

            // Act & Arrange
            var alphabetStringActual = Assert.ThrowsException<ArithmeticException>(() => this.infixNotationService.Calculate(AlphabetString));
            var multiByteStringActual = Assert.ThrowsException<ArithmeticException>(() => this.infixNotationService.Calculate(MultiByteString));
        }

        [TestMethod]
        public void Calculate_UncloseBracketExpresion_ThrowArithmeticException()
        {
            // Arrange
            const string LeftBracketOnly = "( 1 + 1 (";
            const string RightBracketOnly = ") 2 + 2 )";
            const string UncloseBracket = "1 + 2 ) + ( 2 + 3";
            const string ManyLeftBracket = "( 1 + 2 ) + ( 3 + 4";
            const string ManyRightBracket = "( 1 + 2 ) + 3 + 4 )";

            // Act & Arrange
            var leftBracktOnlyActual = Assert.ThrowsException<ArithmeticException>(() => this.infixNotationService.Calculate(LeftBracketOnly));
            var rightBracktOnlyActual = Assert.ThrowsException<ArithmeticException>(() => this.infixNotationService.Calculate(RightBracketOnly));
            var uncloseBracketActual = Assert.ThrowsException<ArithmeticException>(() => this.infixNotationService.Calculate(UncloseBracket));
            var manyLeftBracketActual = Assert.ThrowsException<ArithmeticException>(() => this.infixNotationService.Calculate(ManyLeftBracket));
            var manyRightBracketActual = Assert.ThrowsException<ArithmeticException>(() => this.infixNotationService.Calculate(ManyRightBracket));
        }

        [TestMethod]
        public void Calculate_UncloseBracketExpression_ThrowArithmeticException()
        {
            // Arrange
            const string LeftBracketOnly = "( 1 + 1 (";
            const string RightBracketOnly = ") 2 + 2 )";
            const string UncloseBracket = "1 + 2 ) + ( 2 + 3";
            const string ManyLeftBracket = "( 1 + 2 ) + ( 3 + 4";
            const string ManyRightBracket = "( 1 + 2 ) + 3 + 4 )";

            // Act & Arrange
            var leftBracktOnlyActual = Assert.ThrowsException<ArithmeticException>(() => this.infixNotationService.Calculate(LeftBracketOnly));
            var rightBracktOnlyActual = Assert.ThrowsException<ArithmeticException>(() => this.infixNotationService.Calculate(RightBracketOnly));
            var uncloseBracketActual = Assert.ThrowsException<ArithmeticException>(() => this.infixNotationService.Calculate(UncloseBracket));
            var manyLeftBracketActual = Assert.ThrowsException<ArithmeticException>(() => this.infixNotationService.Calculate(ManyLeftBracket));
            var manyRightBracketActual = Assert.ThrowsException<ArithmeticException>(() => this.infixNotationService.Calculate(ManyRightBracket));
        }

        [TestMethod]
        public void Calculate_OneBinaryOperator_ReturnCalculatedResut()
        {
            // Arrange
            const double Epsilon = 5.0e-15;

            const string AddIntegersExpression = "1 + 20";
            const string AddRealNumbersExpression = "10.2 + 1.38";
            const string SubtractIntegersExpression = "100 - 33";
            const string SubtractRealNumbersExpression = "11.48 - 1.3";
            const string MultiplyIntegersExpression = "100 33 *";
            const string MultiplyRealNumbersExpression = "11.48 * 1.3";
            const string DivideIntegersExpression = "100 / 20";
            const string DivideRealNumbersExpression = "11.48 / 1.3";

            // Act
            var addIntegersActual = this.infixNotationService.Calculate(AddIntegersExpression);
            var addRealNumbersActual = this.infixNotationService.Calculate(AddRealNumbersExpression);
            var subtractIntegersActual = this.infixNotationService.Calculate(SubtractIntegersExpression);
            var subractRealNumbersActual = this.infixNotationService.Calculate(SubtractRealNumbersExpression);
            var multiplyIntegersActual = this.infixNotationService.Calculate(MultiplyIntegersExpression);
            var multiplyRealNumbersActual = this.infixNotationService.Calculate(MultiplyRealNumbersExpression);
            var divideIntegersActual = this.infixNotationService.Calculate(DivideIntegersExpression);
            var divideRealNumbersActual = this.infixNotationService.Calculate(DivideRealNumbersExpression);

            // Assert
            Assert.AreEqual(21, (int)addIntegersActual);
            Assert.AreEqual(11.58, addRealNumbersActual, Epsilon);
            Assert.AreEqual(67, (int)subtractIntegersActual);
            Assert.AreEqual(10.18, subractRealNumbersActual, Epsilon);
            Assert.AreEqual(3300, (int)multiplyIntegersActual);
            Assert.AreEqual(14.924, multiplyRealNumbersActual, Epsilon);
            Assert.AreEqual(5, (int)divideIntegersActual);
            Assert.AreEqual(8.830769230769231, divideRealNumbersActual, Epsilon);
        }

        [TestMethod]
        public void Calculate_ContainsRoundBracket_ReturnCorrectResult()
        {
            // Arrange
            const double Epsilon = 5.0E-15;

            const string StartBracketExpression = "( 50 +  100 ) * 30";
            const string IntermidiateBracketExpression = "50 * ( 100 + 30 ) - 10";
            const string EndBracketExpression = "( 50 +  100 ) / 30";
            const string MultiBracketExpression = "( 10 + 20 ) * ( 110 - 10 ) / 4000";

            // Act
            var startBracketActual = this.infixNotationService.Calculate(StartBracketExpression);
            var intermidiateBracketActual = this.infixNotationService.Calculate(IntermidiateBracketExpression);
            var endBracketActual = this.infixNotationService.Calculate(EndBracketExpression);
            var multiBracketActual = this.infixNotationService.Calculate(MultiBracketExpression);

            // Assert
            Assert.AreEqual(4500, (int)startBracketActual);
            Assert.AreEqual(6490, (int)intermidiateBracketActual);
            Assert.AreEqual(5, (int)endBracketActual);
            Assert.AreEqual(0.75, multiBracketActual, Epsilon);
        }

        [TestMethod]
        public void Calculate_ContainsNestedRoundBracket_ReturnCorrectResult()
        {
            // Arrange
            const double Epsilon = 5.0e-15;

            const string SequentialOpenBracketExpression = "( ( 50 +  100 ) * 60 ) * 30";
            const string IntermidiateBracketExpression = "( 50 * ( 100 + 30 ) / 10 ) / 10";
            const string SequentialCloseBracketExpression = "( 10 * ( 50 +  100 ) ) * 20";
            const string MultipleNestBracketExpression = "3 / ( ( ( 6 + 3 ) / 3 ) * 10 )";

            // Act
            var sequntialOpenBracketActual = this.infixNotationService.Calculate(SequentialOpenBracketExpression);
            var intermidiateBracketActual = this.infixNotationService.Calculate(IntermidiateBracketExpression);
            var sequentialCloseBracketActual = this.infixNotationService.Calculate(SequentialCloseBracketExpression);
            var multipleNestBracketActual = this.infixNotationService.Calculate(MultipleNestBracketExpression);

            // Assert
            Assert.AreEqual(270000, (int)sequntialOpenBracketActual);
            Assert.AreEqual(65, (int)intermidiateBracketActual);
            Assert.AreEqual(30000, (int)sequentialCloseBracketActual);
            Assert.AreEqual(0.1, multipleNestBracketActual, Epsilon);
        }

        #endregion
    }
}