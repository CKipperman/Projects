'use strict';

const interestCalculator = (function() {
    let rate = 0;
    let years = 0;

    return {
        /*getRate() {
            return rate;
        },*/
        setRate(r) {
            if (r <= 0) {
                throw new Error('Rate must be above zero.')
            }
            rate = r;
            return this;
        },
        /*getYears() {
            return years;
        },*/
        setYears(y) {
            if(y <= 0) {
                throw new Error('Number of years must be above zero.')
            }
            years = y;
            return this;
        },
        calculateInterest(principal) {
            if (principal <= 0) {
                throw new Error('Principal must be above zero.');
            }
            let total = principal;
            for (let i = 0; i < years; i++) {
                total += total * rate;
            }
            return total - principal;
        }
    };
}());

console.log('Your interest is $' + interestCalculator.setRate(.1).setYears(2).calculateInterest(100));