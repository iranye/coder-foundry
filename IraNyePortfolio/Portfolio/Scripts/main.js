$(document).ready(function () {
    $("#button-factorial").click(function () {
        $("#output-factorial").text("");
        $("#error-factorial").text("");

        var number = Number($("#input-factorial").val());

        var maxAllowed = 120;
        if (isNaN(number) || number > maxAllowed || number < 1) {
            $("#error-factorial").text("Please enter a positive number up to " + maxAllowed);
            return;
        }
        
        var result = 1;
        for (var n = result; n <= number; n++) {
            result *= n;
        }

        var message = `${number}! = ${result}`;
        $("#output-factorial").text(message);
    });

    $("#button-facts-from-five").click(function () {
        $("#error-facts-from-five").text("");
        $(".result-box").css('visibility', 'hidden');

        var number1 = Number($("#input-facts-from-five-1").val());
        var number2 = Number($("#input-facts-from-five-2").val());
        var number3 = Number($("#input-facts-from-five-3").val());
        var number4 = Number($("#input-facts-from-five-4").val());
        var number5 = Number($("#input-facts-from-five-5").val());

        var numbsArr = [number1, number2, number3, number4, number5];

        if (containsNonNumbers(numbsArr)) {
            $("#error-facts-from-five").text("one or more entries are invalid");
            return;
        }

        if (containsOverflowNumber(numbsArr)) {
            var message = "One or more entries are too big or small. ";
            message += "Each number must be between " + Number.MIN_SAFE_INTEGER + " and " + Number.MAX_SAFE_INTEGER;
            $("#error-facts-from-five").text(message);
            return;
        }
        
        $(".result-box").css('visibility', 'visible');

        var min = getMin(numbsArr);
        $("#output-facts-from-five-smallest").text(min);

        var max = getMax(numbsArr);
        $("#output-facts-from-five-greatest").text(max);

        var sum = sumAll(numbsArr);
        $("#output-facts-from-five-sum").text(sum);

        var mean = getMean(numbsArr);
        $("#output-facts-from-five-mean").text(mean);

        var product = getProduct(numbsArr);
        $("#output-facts-from-five-product").text(product);
    });

    $("#button-fizzbuzz").click(function () {
        console.log("Running: fizzbuzz");

        $("#error-fizzbuzz").text("");
        $("#output-fizzbuzz").text("");
        $(".result-box").css('visibility', 'hidden');

        var number1 = Number($("#input-fizzbuzz-1").val());
        var number2 = Number($("#input-fizzbuzz-2").val());

        var numbsArr = [number1, number2];

        if (containsNonNumbers(numbsArr)) {
            $("#error-fizzbuzz").text("one or more entries are invalid");
            return;
        }

        $(".result-box").css('visibility', 'visible');
        var result = getFizzBuzz(number1, number2);
        console.log(result);
        $("#output-fizzbuzz").text(result);
    });

    $("#btn-prime-factorization").click(function () {
        console.log("Running: prime-factorization");

        $("#error-prime-factorization").text("");
        $("#output-prime-factorization").text("");

        var number = Number($("#input-prime-factorization").val());

        var maxAllowed = Number.MAX_SAFE_INTEGER;
        if (isNaN(number) || number > maxAllowed || number < 1) {
            $("#error-prime-factorization").text("Please enter a positive number up to " + maxAllowed);
            return;
        }

        var result = getPrimeFactors(number);
        console.log(result.length);
        console.log(result);
        var message = "";
        if (result.length === 1) {
            message = `${result[0]} is a prime number!`;
        } else {
            message = `${number} = ${result.join(" * ")}`;
        }

        $("#output-prime-factorization").text(message);
    });

    $("#button-palindrome").click(function () {
        var input = $("#input-palindrome").val().trim();
        var word = input.toUpperCase();
        console.log(`massaged input: ${word}`);

        $("#error-palindrome").text("");
        $("#output-palindrome").text("");

        if (word.length === 0) {
            $("#error-palindrome").text("Please enter a word");
            return;
        }
        var charArr = word.split('').reverse();
        var reversed = charArr.join("");

        var result = "not a palindrome";
        if (reversed === word) {
            result = `'${input}' is a palindrome!`;
        }

        $("#output-palindrome").text(result);
    });

    var defaultSearchReplaceInput = `.?(o)(.*)~~$1 => $2
foo
toe
octopus
razor`;

    $("#btn-search-replace-change-input").click(function () {
        console.log("You clicked: btn-search-replace-change-input");
        console.log(`defaultSearchReplaceInput: ${defaultSearchReplaceInput}`);

        $("#input-search-replace").value = "";
        $("#input-search-replace").value = defaultSearchReplaceInput;
        // $("#input-search-replace").text(defaultSearchReplaceInput);
    });

    $("#btn-search-replace").click(function () {
        var input = $("#input-search-replace").val().trim();

        $("#error-search-replace").text("");
        $("#output-search-replace").text("");

        if (input.length === 0) {
            $("#input-search-replace").text(defaultSearchReplaceInput);
            return;
        }
        var lines = input.split("\n");
        console.log(`lines found: ${lines.length}`);
        if (lines.length < 2) {
            $("#error-search-replace")
                .text(
                    "Please enter at least 2 lines.  Line 1 should contain the pattern and lines 2 through n are the input lines.");
            return;
        }

        var newText = "";
        
        if (lines[0].match("~~")) {
            var a = lines[0].split("~~");
            if (a.length < 2) {
                $("#error-search-replace")
                    .text(
                        "Invalid pattern on Line 1, which should contain the pattern and lines 2 through n are the input lines.");
                return;
            }

            var re = a[0].trim();
            if (!re.length) {
                $("#error-search-replace")
                    .text(
                        "Missing the search pattern that should be to the left of the ~~.");
                return;
            }

            newText = a[1].trim();
            console.log(`newText: ${newText}`);

            if (!newText.length) {
                $("#error-search-replace")
                    .text(
                        "Missing the replace pattern that should be to the right of the ~~.");
                return;
            }
        }

        var pattern = new RegExp(re);
        console.log(`pattern: ${pattern}`);
        for (var i = 1; i < lines.length; i++) {
            var line = lines[i].trim();
            if (line.length === 0) { // skip whitespace-only lines
                continue;
            }
            console.log(`line {i} (before): ${line}`);
            line = line.replace(pattern, newText);
            console.log(`line {i} (after): ${line}`);
            line += "\r\n";
            $("#output-search-replace").append(line);
        }
    });

    $("#btn-add-task").click(function () {
        console.log("Adding Task");
        var taskControl = $("#taskName");

        $("#error-todo-list").text("");
        $(".result-box").css('visibility', 'visible');

        var taskMassaged = taskControl.val().trim();
        if (taskMassaged.length === 0) {
            $("#error-todo-list").text("Please enter a task");
            return;
        }

        createTask(taskMassaged);
        taskControl.val("");
        taskControl.focus();
    });

    function createTask(item) {
        var markup = `<li><button style="width: 22px; height: 25px;" onclick="deleteTask(this)" title="Mark as Done" alt="Mark as Done">D</button> ${item}</li>`;
        var tasks = $("#tasks");
        tasks.after(markup);
    }

    $("#modal-facts-from-five .btn-toggle-code").click(function () {
        $("#modal-facts-from-five .div-code").toggle();

    });

    $("#modal-factorial .btn-toggle-code").click(function () {
        $("#modal-factorial .div-code").toggle();

    });

    $("#modal-fizzbuzz .btn-toggle-code").click(function () {
        $("#modal-fizzbuzz .div-code").toggle();

    });

    $("#modal-palindrome .btn-toggle-code").click(function () {
        $("#modal-palindrome .div-code").toggle();

    });

    $("#modal-prime-factorization .btn-toggle-code").click(function () {
        $("#modal-prime-factorization .div-code").toggle();

    });

    $("#modal-search-replace .btn-toggle-code").click(function () {
        $("#modal-search-replace .div-code").toggle();

    });

    $("#modal-todo-list .btn-toggle-code").click(function () {
        $("#modal-todo-list .div-code").toggle();
    });
});

function deleteTask(taskToDelete) {
    console.log("deleting! " + taskToDelete);
    // TODO: Use strikethrough & toggle instead
    taskToDelete.parentElement.remove();
}

function containsNonNumbers(arr) {
    var i;
    var result = false;
    for (i = 0; i < arr.length; i++) {
        if (isNaN(arr[i])) {
            result = true;
            console.log(arr[i] + " isNaN");
            break;
        }
    }
    return result;
}

function containsOverflowNumber(arr) {
    var i;
    var result = false;
    for (i = 0; i < arr.length; i++) {
        if (!(arr[i] <= Number.MAX_SAFE_INTEGER)) {
            result = true;
            break;
        }
        if (!(arr[i] >= Number.MIN_SAFE_INTEGER)) {
            result = true;
            break;
        }
    }
    return result;
}

function getMin(arr) {
    var i;
    var min = Number.MAX_SAFE_INTEGER;
    for (i = 0; i < arr.length; i++) {
        if (min > arr[i]) {
            min = arr[i];
        }
    }
    return min;
}

function getMax(arr) {
    var i;
    var max = Number.MIN_SAFE_INTEGER;
    for (i = 0; i < arr.length; i++) {
        if (max < arr[i]) {
            max = arr[i];
        }
    }
    return max;
}

function getMean(arr) {
    var sum = sumAll(arr);
    return sum / arr.length;
}

function sumAll(arr) {
    var i;
    var sum = 0;
    for (i = 0; i < arr.length; i++) {
        sum += arr[i];
    }
    return sum;
}

function getProduct(arr) {
    var i;
    var product = 1;
    for (i = 0; i < arr.length; i++) {
        product *= arr[i];
    }
    return product;
}

function getFizzBuzz(fizzVal, buzzVal) {
    var result = "";

    for (var i = 1; i <= 100; i++) {
        var foo = "";
        if (i % fizzVal === 0) {
            foo = "Fizz";
        }
        if (i % buzzVal === 0) {
            foo += "Buzz";
        }
        if (foo.length === 0) {
            foo = i;
        }
        result += foo + " ";
    }
    return result;
}

function getPrimeFactors(input) {
    var primeFactors = [];
    var factor = 2;
    while (input % factor === 0)
    {
        primeFactors.push(factor);
        input /= factor;
    }

    factor = 3;
    while (factor * factor <= input) {
        if (input % factor === 0) {
            primeFactors.push(factor);
            input /= factor;
        }
        else {
            factor += 2;
        }

    }
    if (input > 1) primeFactors.push(input);
    return primeFactors;
}