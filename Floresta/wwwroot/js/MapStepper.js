
var numStepeerInput = document.querySelector("#plantCountInput");
var thisAction = document.querySelectorAll(".count_btn");
var StepperSetings = {
    min: parseInt(numStepeerInput.min, 10),
    max: parseInt(numStepeerInput.max, 10),
    step: parseInt(numStepeerInput.step, 10)
};

thisAction.forEach(btnAction => {
    btnAction.addEventListener('click', {
        handleEvent(event) {
            switch (btnAction.innerHTML) {
                case '+': updateVal("increment");
                    break;
                case '-': updateVal("decrement");
                    break;
                default:
                    break;
            }
        }
    });
});
numStepeerInput.addEventListener("blur", function () {
        updateVal("entered");
});
numStepeerInput.addEventListener("keypress", function (e) {
    if (e.key === 'Enter') {
        updateVal("entered");
    }
});
function updateVal(action) {
    var tempValue = parseInt(numStepeerInput.value, 10);
    var newValue = parseInt(numStepeerInput.value, 10);
    if (typeof tempValue === "number"  && action == "entered" || typeof tempValue === "string") {
        if (typeof tempValue === "string"|| Number.isNaN(tempValue)) {
            numStepeerInput.value = StepperSetings.min;
        }
        else {
            if (tempValue > StepperSetings.max) {
                numStepeerInput.value = StepperSetings.max;
            }
            else if (tempValue < StepperSetings.min) {
                numStepeerInput.value = StepperSetings.min;
            }
            else {
                numStepeerInput.value = tempValue;
            }
        }
    }
    else {
        if (action == "increment" && newValue < StepperSetings.max) {
            newValue = newValue + StepperSetings.step;
        }
        else if (action == "decrement" && newValue > StepperSetings.min) {
            newValue = newValue - StepperSetings.step;
        }
        numStepeerInput.value = newValue;
    }
}
