import { Directive, ElementRef, Renderer2 } from '@angular/core';

@Directive({ selector: '[dragging]' })
export class DragDirective {
    constructor(private elem: ElementRef, private renderer: Renderer2) {}
    ngAfterViewInit() {
        var elem = this.elem.nativeElement, headerElem;
        var pos1 = 0, pos2 = 0, pos3 = 0, pos4 = 0;

        if (elem.firstElementChild && elem.firstElementChild.firstElementChild) {
            headerElem = elem.firstElementChild.firstElementChild;
            this.renderer.setStyle(headerElem, 'cursor', 'move');
            this.renderer.setAttribute(headerElem,'title', 'Drag and move');
            /* if present, the header is where you move the DIV from:*/
            headerElem.onmousedown = dragMouseDown;
        } else {
            /* otherwise, move the DIV from anywhere inside the DIV:*/
            elem.onmousedown = dragMouseDown;
        }

        function dragMouseDown(e) {
            e = e || window.event;
            e.preventDefault();
            // get the mouse cursor position at startup:
            pos3 = e.clientX;
            pos4 = e.clientY;
            document.onmouseup = closeDragElement;
            // call a function whenever the cursor moves:
            document.onmousemove = elementDrag;
        }

        function elementDrag(e) {
            e = e || window.event;
            e.preventDefault();
            // calculate the new cursor position:
            pos1 = pos3 - e.clientX;
            pos2 = pos4 - e.clientY;
            pos3 = e.clientX;
            pos4 = e.clientY;
            // set the element's new position:
            elem.style.top = (elem.offsetTop - pos2) + "px";
            elem.style.left = (elem.offsetLeft - pos1) + "px";
        }

        function closeDragElement() {
            /* stop moving when mouse button is released:*/
            document.onmouseup = null;
            document.onmousemove = null;
        }
    }
}