function simulatePayoff(initialBalance, apr, monthlyFee, minPayment) {
    const monthlyRate = apr / 12 / 100;
    let balance = initialBalance;
    let totalPaid = 0;
    let months = 0;

    while (balance > 0) {
        months++;
        const balanceBeforeInterest = balance + monthlyFee;
        const interest = Math.round(balanceBeforeInterest * monthlyRate * 100) / 100;
        const balanceBeforePayment = balanceBeforeInterest + interest;
        let payment = balanceBeforePayment - minPayment < 0
            ? balanceBeforePayment
            : minPayment;
        balance = Math.round((balanceBeforePayment - payment) * 100) / 100;
        totalPaid += payment;
        
        if (months > 1000) break;
    }
    return { months, totalPaid: Math.round(totalPaid * 100) / 100 };
}

const stellar = simulatePayoff(1000, 16, 10,  35);
const vortex = simulatePayoff(1000, 21, 0,  20);

console.log("=== Stellar Horizon Visa ===");
console.log(`Months to pay off: ${stellar.months}`);
console.log(`Total amount paid: $${stellar.totalPaid}`);

console.log("\n=== Vortex Elite Mastercard ===");
console.log(`Months to pay off: ${vortex.months}`);
console.log(`Total amount paid: $${vortex.totalPaid}`);