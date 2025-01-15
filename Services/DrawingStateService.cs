using Microsoft.JSInterop;
using wubzy.Shared;

namespace wubzy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class DrawingStateService
    {
        private readonly Stack<Line> _undoStack = new Stack<Line>();
        private readonly Stack<Line> _redoStack = new Stack<Line>();

        private Line _currentLine;

        public DrawingStateService()
        {
            _currentLine = new Line("black", 2); // default color and thickness
        }

        public void StartNewLine(string color, double thickness)
        {
            _currentLine = new Line(color, thickness);
        }

        public void AddPoint(double x, double y)
        {
            _currentLine.AddPoint(x, y);
        }

        public Line GetCurrentLine()
        {
            return _currentLine;
        }

        public void SaveState()
        {
            _undoStack.Push(new Line(_currentLine.Color, _currentLine.Thickness) { Points = new List<(double, double)>(_currentLine.Points) });
            _redoStack.Clear();
        }
        
        public Line Undo()
        {
            if (_undoStack.Count > 0)
            {
                var previousLine = _undoStack.Pop();
                _redoStack.Push(new Line(_currentLine.Color, _currentLine.Thickness) { Points = new List<(double, double)>(_currentLine.Points) });
                _currentLine = previousLine;
            }
            
            return _currentLine;
        }

        public Line Redo()
        {
            if (_redoStack.Count > 0)
            {
                var nextLine = _redoStack.Pop();
                _undoStack.Push(new Line(_currentLine.Color, _currentLine.Thickness) { Points = new List<(double, double)>(_currentLine.Points) });
                _currentLine = nextLine;
            }
            return _currentLine;
        }

        public async Task DrawAllLinesAsync(string canvasId, IJSRuntime jsRuntime)
        {
            await DrawLineAsync(canvasId, jsRuntime, _currentLine);

            foreach (var line in _undoStack)
            {
                await DrawLineAsync(canvasId, jsRuntime, line);
            }
        }

        private async Task DrawLineAsync(string canvasId, IJSRuntime jsRuntime, Line line)
        {
            if (line.Points.Count < 2) return;

            await jsRuntime.InvokeVoidAsync("canvasInterop.moveTo", canvasId, line.Points[0].X, line.Points[0].Y);

            for (int i = 1; i < line.Points.Count; i++)
            {
                await jsRuntime.InvokeVoidAsync("canvasInterop.lineTo", canvasId, line.Points[i].X, line.Points[i].Y);
            }

            await jsRuntime.InvokeVoidAsync("canvasInterop.stroke", canvasId, line.Color, line.Thickness);
        }
    }
}