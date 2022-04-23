﻿using Microsoft.EntityFrameworkCore;
using System.Linq;
using BugTracker.Data;
using BugTracker.ViewModels;
using BugTracker.Models;

namespace BugTracker.Services
{
    public class BoardService
    {
        private readonly ApplicationDbContext _dbContext;

        public BoardService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public BoardList ListBoard()
        {
            var model = new BoardList();

            foreach (var board in _dbContext.Boards)
            {
                model.Boards.Add(new BoardList.Board
                {
                    Id = board.Id,
                    Title = board.Title
                });
            }

            return model;
        }

        public BoardView GetBoard(int id)
        {
            var model = new BoardView();

            var board = _dbContext.Boards
                .Include(b => b.Columns)
                .ThenInclude(c => c.Cards)
                .SingleOrDefault(x => x.Id == id);

            if (board == null)
                return model;
            model.Id = board.Id;
            model.Title = board.Title;

            foreach (var column in board.Columns)
            {
                var modelColumn = new BoardView.Column
                {
                    Title = column.Title,
                    Id = column.Id
                };

                foreach (var card in column.Cards)
                {
                    var modelCard = new BoardView.Card
                    {
                        Id = card.Id,
                        Content = card.Contents
                    };

                    modelColumn.Cards.Add(modelCard);
                }

                model.Columns.Add(modelColumn);
            }

            return model;
        }

        public void AddCard(AddCard viewModel)
        {
            var board = _dbContext.Boards
                .Include(b => b.Columns)
                .SingleOrDefault(x => x.Id == viewModel.Id);

            if (board != null)
            {
                var firstColumn = board.Columns.FirstOrDefault();
                var secondColumn = board.Columns.FirstOrDefault();
                var thirdColumn = board.Columns.FirstOrDefault();
                var FourthColumn = board.Columns.FirstOrDefault();

                if (firstColumn == null || secondColumn == null || thirdColumn == null)
                {
                    firstColumn = new Models.Column { Title = "Todo" };
                    secondColumn = new Models.Column { Title = "Doing" };
                    thirdColumn = new Models.Column { Title = "Test" };
                    FourthColumn = new Models.Column { Title = "Done" };
                    board.Columns.Add(firstColumn);
                    board.Columns.Add(secondColumn);
                    board.Columns.Add(thirdColumn);
                    board.Columns.Add(FourthColumn);
                }

                firstColumn.Cards.Add(new Models.Card
                {
                    Contents = viewModel.Contents
                });
            }

            _dbContext.SaveChanges();
        }

        public void AddBoard(NewBoard viewModel, int? id)
        {
            var project = _dbContext.Projects.Find(viewModel.ProjID);
            Board brd = new();
            brd.Project = project;
            brd.Title = viewModel.Title;
            //viewModel.Project = project;



            _dbContext.Boards.Add(brd);

            _dbContext.SaveChanges();
        }

        public void DeleteBoard(int id)
        {
            var board = _dbContext.Boards
                .Include(i => i.Columns)
                .ThenInclude(x => x.Cards)
                .FirstOrDefault(i => i.Id == id);

            _dbContext.Boards.Remove(board);
            _dbContext.SaveChanges();
        }

        public void Move(MoveCardCommand command)
        {
            var card = _dbContext.Cards.SingleOrDefault(x => x.Id == command.CardId);
            card.ColumnId = command.ColumnId;
            _dbContext.SaveChanges();
        }
    }
}
