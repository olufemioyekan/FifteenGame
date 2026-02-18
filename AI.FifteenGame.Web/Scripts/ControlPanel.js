(function ($) {
    "use strict";

    var defaultTileColor = "#ECE0CA";
    var defaultBoardColor = "#B9AEA1";
    var defaultEmptyColor = "#C9BCAD";
    var solvedOrder = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, ""];

    // Tile color picker
    $("#tileColorPicker").on("input", function () {
        var color = $(this).val();
        $(".tile").not(".empty").css("background-color", color);
    });

    // Board color picker
    $("#boardColorPicker").on("input", function () {
        var color = $(this).val();
        $("#board").css("background-color", color);
    });

    // Difficulty slider label update
    $("#difficultySlider").on("input", function () {
        $("#difficultyLabel").text($(this).val());
    });

    // Helper: find the grid index of the empty tile
    function findEmptyIndex($tiles) {
        var idx = 0;
        $tiles.children(".tile").each(function (i) {
            if ($(this).hasClass("empty")) {
                idx = i;
            }
        });
        return idx;
    }

    // Helper: get valid neighbor indices for a position on a 4x4 grid
    function getNeighbors(index) {
        var neighbors = [];
        var row = Math.floor(index / 4);
        var col = index % 4;
        if (row > 0) neighbors.push(index - 4); // up
        if (row < 3) neighbors.push(index + 4); // down
        if (col > 0) neighbors.push(index - 1); // left
        if (col < 3) neighbors.push(index + 1); // right
        return neighbors;
    }

    // Helper: swap two tile elements by grid index
    function swapTiles($tiles, idxA, idxB) {
        var children = $tiles.children(".tile");
        var $a = children.eq(idxA);
        var $b = children.eq(idxB);
        var tempHtml = $a.html();
        var tempClass = $a.attr("class");
        $a.html($b.html());
        $a.attr("class", $b.attr("class"));
        $b.html(tempHtml);
        $b.attr("class", tempClass);
    }

    // Shuffle: perform valid random moves based on difficulty level
    $("#shuffleBtn").on("click", function () {
        var difficulty = parseInt($("#difficultySlider").val(), 10);
        // Map difficulty 1-5 to number of random moves
        var moveCounts = { 1: 5, 2: 15, 3: 40, 4: 80, 5: 150 };
        var totalMoves = moveCounts[difficulty] || 40;

        var $tiles = $("#tiles");
        var lastMove = -1; // track previous empty index to avoid undoing moves

        for (var m = 0; m < totalMoves; m++) {
            var emptyIdx = findEmptyIndex($tiles);
            var neighbors = getNeighbors(emptyIdx);

            // Filter out the last position to avoid immediately undoing the previous move
            var candidates = neighbors.filter(function (n) { return n !== lastMove; });
            if (candidates.length === 0) {
                candidates = neighbors;
            }

            var pick = candidates[Math.floor(Math.random() * candidates.length)];
            lastMove = emptyIdx;
            swapTiles($tiles, emptyIdx, pick);
        }

        // Apply current color selections
        var tileColor = $("#tileColorPicker").val();
        $(".tile").not(".empty").css("background-color", tileColor);

        // Hide overlays
        $(".overlay").hide();
    });

    // Reset Game
    $("#resetGameBtn").on("click", function () {
        var $tiles = $("#tiles");
        var tileElements = $tiles.children(".tile").toArray();

        // Shuffle tiles using Fisher-Yates
        for (var i = tileElements.length - 1; i > 0; i--) {
            var j = Math.floor(Math.random() * (i + 1));
            $tiles.append(tileElements[j]);
            var temp = tileElements[i];
            tileElements[i] = tileElements[j];
            tileElements[j] = temp;
        }

        // Ensure solvable: count inversions (ignoring empty tile)
        var values = [];
        $tiles.children(".tile").each(function () {
            var text = $(this).text().trim();
            if (text !== "") {
                values.push(parseInt(text, 10));
            }
        });

        var inversions = 0;
        for (var a = 0; a < values.length; a++) {
            for (var b = a + 1; b < values.length; b++) {
                if (values[a] > values[b]) {
                    inversions++;
                }
            }
        }

        // For a 4x4 grid: find the row of the empty tile from the bottom
        var emptyIndex = 0;
        $tiles.children(".tile").each(function (index) {
            if ($(this).hasClass("empty")) {
                emptyIndex = index;
            }
        });
        var emptyRowFromBottom = 4 - Math.floor(emptyIndex / 4);

        // Solvable when: (inversions even && empty on odd row from bottom)
        // or (inversions odd && empty on even row from bottom)
        var solvable = (inversions % 2 === 0) === (emptyRowFromBottom % 2 === 1);

        if (!solvable) {
            // Swap first two non-empty tiles to fix parity
            var children = $tiles.children(".tile");
            var first = -1;
            var second = -1;
            children.each(function (idx) {
                if (!$(this).hasClass("empty")) {
                    if (first === -1) {
                        first = idx;
                    } else if (second === -1) {
                        second = idx;
                    }
                }
            });
            if (first !== -1 && second !== -1) {
                var $first = children.eq(first);
                var $second = children.eq(second);
                var tempHtml = $first.html();
                $first.html($second.html());
                $second.html(tempHtml);
            }
        }

        // Reset colors to current picker values
        var tileColor = $("#tileColorPicker").val();
        var boardColor = $("#boardColorPicker").val();
        $(".tile").not(".empty").css("background-color", tileColor);
        $("#board").css("background-color", boardColor);

        // Hide overlays
        $(".overlay").hide();
    });

    // Solve Game - animate tiles to solved order
    $("#solveGameBtn").on("click", function () {
        var $tiles = $("#tiles");
        var $btn = $(this);
        $btn.prop("disabled", true);

        // Build solved state
        var solvedTiles = [];
        $tiles.children(".tile").each(function () {
            var text = $(this).text().trim();
            var val = text === "" ? 16 : parseInt(text, 10);
            solvedTiles.push({ element: this, value: val });
        });

        // Sort by value to get solved order
        solvedTiles.sort(function (a, b) { return a.value - b.value; });

        // Animate by appending in order with a small delay
        var delay = 0;
        $.each(solvedTiles, function (index, item) {
            setTimeout(function () {
                $tiles.append(item.element);
                if (index === solvedTiles.length - 1) {
                    $btn.prop("disabled", false);
                }
            }, delay);
            delay += 80;
        });
    });

})(jQuery);
