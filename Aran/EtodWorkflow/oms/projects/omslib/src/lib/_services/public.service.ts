import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class PublicService {

    constructor() { }

    countryCodes = [
        {
            "code": "+994",
            "name": "Azerbaijan"
        },
        {
            "code": "+7 7",
            "name": "Kazakhstan"
        }
    ]
    months = [
        {
            'long': 'January',
            'short': 'Jan',
            'val': '1'
        },
        {
            'long': 'February',
            'short': 'Feb',
            'val': '2'
        },
        {
            'long': 'March',
            'short': 'Mar',
            'val': '3'
        },
        {
            'long': 'April',
            'short': 'Apr',
            'val': '4'
        },
        {
            'long': 'May',
            'short': 'May',
            'val': '5'
        },
        {
            'long': 'June',
            'short': 'June',
            'val': '6'
        },
        {
            'long': 'July',
            'short': 'July',
            'val': '7'
        },
        {
            'long': 'August',
            'short': 'Aug',
            'val': '8'
        },
        {
            'long': 'September',
            'short': 'Sept',
            'val': '9'
        },
        {
            'long': 'October',
            'short': 'Oct',
            'val': '10'
        },
        {
            'long': 'November',
            'short': 'Nov',
            'val': '11'
        },
        {
            'long': 'December',
            'short': 'Dec',
            'val': '12'
        }
    ]

    getYearList = function (first = 1905, second = new Date().getFullYear()) {
        var arr = Array();
        for (var i = first; i <= second; i++) arr.push(i.toString());
        return arr;
    }

    getDayListOfMonth = function (year = new Date().getFullYear(), month = new Date().getMonth()) {
        var days = [], dayCount = new Date(year, month, 0).getDate();
        for (let i = 0; i < dayCount; i++) days.push((i + 1).toString());
        return days;
    }

    alertMsg(msg, duration = 3000, type: string = "success") {
        var currentTop = 60;
        var alertBody = document.getElementsByClassName('oms-alert-body');
        if (alertBody && alertBody.length > 0) {
            var lastAlert = alertBody[alertBody.length - 1];
            currentTop = parseInt(lastAlert['style']['top']) + 60;
        }
        var el = document.createElement("div");
        el.setAttribute("style", "position:fixed;top:"+currentTop+"px;right:10px;z-index:10000");
        el.setAttribute("class", "oms-alert-body alert alert-" + type);
        // el.classList.add("alert");
        el.innerHTML = msg;
        document.body.appendChild(el);

        setTimeout(function () {
            el.parentNode.removeChild(el);
        }, duration);
    }

    parseJwt(token) {
        var base64Url = token.split('.')[1];
        var base64 = base64Url.replace('- ', '+').replace('_', '/');
        return JSON.parse(window.atob(base64));
    };

    validateEmail(email) {
        var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        console.log(re.test(String(email).toLowerCase()));
        return re.test(String(email).toLowerCase());
    }

    getDayStrFromToday(days: number = 0): string {
        var result: any = new Date();
        result.setDate(result.getDate() + days);
        result = result.toJSON().slice(0, 10);
        console.log(result);
        return result;
    }

    getTodayStrReversed(): string {
        // 17-12-2018
        return new Date().toJSON().slice(0, 10).split('-').reverse().join('-');
    }

    getUtcTime(){
        return new Date().getTime();
    }
    //YYYY-MM-DD, only allows dates in the 20th and 21st centuries (?:19|20)[0-9]{2}-(?:(?:0[1-9]|1[0-2])-(?:0[1-9]|1[0-9]|2[0-9])|(?:(?!02)(?:0[1-9]|1[0-2])-(?:30))|(?:(?:0[13578]|1[02])-31))

    sortByKey(array: any[], key) {
        // return array.sort((a, b) => a[key].localeCompare(b[key]));
        return array.sort((a, b) => a[key].localeCompare(b[key]));
    }

    penetrateFixedTo(num) {
        var sign = Math.sign(num);
        var result = sign * Math.ceil(Math.abs(num) * 10) / 10;
        if (sign === -1) {
            result = sign * Math.floor(Math.abs(num) * 10) / 10;
        }
        return result;
    }

}