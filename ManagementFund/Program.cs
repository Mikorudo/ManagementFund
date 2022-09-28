using System;

namespace ManagementFund
{
	internal class Program
	{
		enum Action
		{
			upLevel,
			downLevel,
			stayLevel
		}
		class Game
		{
			private static bool[] pattern = new bool[] {
				true, true, true, true, true,
				false, false, false, false, false, false,
				true, true,
				false, false, false, false,
				true, true, true, true,
				false, true, false, false
			};

			public int level;
			public int gameItertation;
			public Game()
			{
				level = 0;
				gameItertation = 0;
			}
			private Action GenerateHint()
			{
				if (level < 0 || level >= 5)
					throw new Exception("Уровень =" + level);
				Random random = new Random();
				double chance = random.NextDouble();
				if (level == 0)
					return Action.upLevel;
				else if (level == 1)
				{
					if (chance < 1.0 / 3.0)
						return Action.upLevel;
					else if (chance < 2.0 / 3.0)
						return Action.stayLevel;
					else
						return Action.downLevel;
				}
				else
				{
					if (chance < 0.5)
						return Action.upLevel;
					else
						return Action.downLevel;
				}
			}
			private void MakeChoice(Action hint)
			{
				if (level < 0 || level >= 5)
					throw new Exception("Уровень =" + level);
				if (level == 0)
					level++;
				else if (level != 1)
				{
					switch (hint)
					{
						case Action.upLevel:
							{
								bool a = pattern[gameItertation];
								if (a)
									level++;
								else
									level--;
							}
							break;
						case Action.downLevel:
							{
								bool a = pattern[gameItertation];
								if (a)
									level--;
								else
									level++;
							}
							break;
						default:
							throw new Exception("Получена подсказка оставаться на уровне, но уровень = " + level);
					}
				}
				else
				{
					Random random = new Random();
					double chance = random.NextDouble();
					switch (hint)
					{
						case Action.upLevel:
							{
								bool a = pattern[gameItertation];
								if (a)
									level++;
								else
								{
									if (chance < 0.5)
										level--;
									else
										level = level;
								}
							}
							break;
						case Action.downLevel:
							{
								bool a = pattern[gameItertation];
								if (a)
									level--;
								else
								{
									if (chance < 0.5)
										level++;
									else
										level = level;
								}
							}
							break;
						case Action.stayLevel:
							{
								bool a = pattern[gameItertation];
								if (a)
									level = level;
								else
								{
									if (chance < 0.5)
										level++;
									else
										level--;
								}
							}
							break;
						default:
							throw new Exception("Получена подсказка оставаться на уровне, но уровень = " + level);
					}
				}
			}
			private void Play()
			{
				Action action = GenerateHint();
				MakeChoice(action);
				gameItertation++;
			}
			public Report PlayGame(int max = 25)
			{
				for (int i = 0; i < max; i++)
				{
					Play();
					if (level == 5)
						return new Report(Winner.First, gameItertation);
				}
				return new Report(Winner.Second, gameItertation);
			}
			public void Reboot()
			{
				level = 0;
				gameItertation = 0;
			}
		}
		enum Winner
		{
			First,
			Second
		}
		class Report
		{
			public Winner winner;
			public int stepNumber;
			public Report(Winner winner, int stepNumber) { this.winner = winner; this.stepNumber = stepNumber;}
		}

		static void GetStats(int gameCount, int max = 25)
		{
			int firstWinCount = 0;
			int secondWinCount = 0;

			int stepNumberSum = 0;

			Game game = new Game();
			for (int i = 0; i < gameCount; i++)
			{
				Report report = game.PlayGame(max);
				switch (report.winner)
				{
					case Winner.First:
						firstWinCount++;
						break;
					case Winner.Second:
						secondWinCount++;
						break;
					default:
						throw new Exception("error");
				}
				stepNumberSum += report.stepNumber;
				game.Reboot();
			}
			double middleStepNumber = stepNumberSum / gameCount;
			Console.WriteLine("Количество игр: " + gameCount);
			Console.WriteLine("Победа второго игрока после " + max + " хода");
			Console.WriteLine("Побед первого игрока: " + firstWinCount);
			Console.WriteLine("Побед второго игрока: " + secondWinCount);
			Console.WriteLine("Среднее число ходов на партию: " + middleStepNumber);
		}

		static void Main(string[] args)
		{
			GetStats(1000);
			Console.WriteLine();
			GetStats(1000, 20);
		}
	}
}
