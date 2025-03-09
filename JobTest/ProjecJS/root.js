const fs = require('fs');
const axios = require('axios');

const filePath = 'test.json';
const outputFilePath = 'response.json';

const allUsers = JSON.parse(fs.readFileSync(filePath, 'utf8'));

const activeUsers = allUsers
    .filter(user => user.isActive)
    .map(user => ({
        name: user.name ? user.name.trim() : null,
        phone: user.phone && user.phone.trim().length >= 11 ? user.phone.trim() : null,
        email: user.email && user.email.includes("@") ? user.email.trim() : null,
        countFriends: user.friends ? user.friends.length : 0,
        friends: user.friends ? user.friends.map(friend => ({ name: friend.name })) : []
    }))
    .filter(user => user.name && user.phone && user.email);

console.log("Отправляем на сервер");

axios.post('http://localhost:5222/api/Analize', activeUsers)
    .then(response => {
        fs.writeFileSync(outputFilePath, JSON.stringify(response.data, null, 2));
        console.log('Данные успешно отправлены и записаны в response.json');
    })
    .catch(error => {
        if (error.response) {
            console.error('Ошибка сервера:', error.response.status, error.response.data);
        } else if (error.request) {
            console.error('Ошибка сети: сервер не отвечает');
        } else {
            console.error('Ошибка:', error.message);
        }
    });