'use strict';

function createAccount() {
    return {
        balance: 0,
        setBalance(amount) {
            this.balance = amount;
        },
        getBalance() {
            return this.balance;
        },
        performTransaction(amount) {
            this.balance += amount;
            return this.balance;
        }
    };
}

// Part B: separate transaction function
function performTransaction(amount) {
    this.balance += amount;
    return this.balance;
}

const account1 = createAccount();
const account2 = createAccount();

account1.setBalance(100);
account2.setBalance(200);

console.log('Part A:');
account1.performTransaction(50); 
account2.performTransaction(-25);
console.log(`Account 1 balance: $${account1.getBalance()}`); // 150
console.log(`Account 2 balance: $${account2.getBalance()}`); // 175

console.log('Part B:');
performTransaction.call(account1, 50);  // deposit
performTransaction.call(account2, -25); // withdrawal
console.log(`Account 1 balance: $${account1.getBalance()}`); // 200
console.log(`Account 2 balance: $${account2.getBalance()}`); // 150

console.log('Part C:');
const depositFiftyInAccount1 = performTransaction.bind(account1, 50);
depositFiftyInAccount1();
depositFiftyInAccount1();
console.log(`Account 1 balance: $${account1.getBalance()}`); // 300