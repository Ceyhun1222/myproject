import { Directive, Input, ElementRef, HostListener, Renderer2 } from '@angular/core';

@Directive({
  selector: '[tooltip]'
})
export class TooltipDirective {
  @Input('tooltip') tooltipTitle: string;
  @Input() placement: string;
  @Input() delay: string;
  tooltip: HTMLElement;
  // 호스트 요소와 tooltip 요소 간의 거리
  offset = 10;

//   https://stackblitz.com/edit/angular-tooltip-directive?file=app%2Fapp.component.ts
  constructor(private el: ElementRef, private renderer: Renderer2) { }

  @HostListener('mouseenter') onMouseEnter() {
    if (!this.tooltip) { this.show(); }
  }

  @HostListener('mouseleave') onMouseLeave() {
    if (this.tooltip) { this.hide(); }
  }

  show() {
    this.create();
    this.setPosition();
    this.renderer.addClass(this.tooltip, 'ng-tooltip-show');
  }

  hide() {
    this.renderer.removeClass(this.tooltip, 'ng-tooltip-show');
    window.setTimeout(() => {
      this.renderer.removeChild(document.body, this.tooltip);
      this.tooltip = null;
    }, 1000);
  }

  create() {
    this.tooltip = this.renderer.createElement('span');

    this.renderer.appendChild(
      this.tooltip,
      this.renderer.createText(this.tooltipTitle) // textNode
    );

    this.renderer.appendChild(document.body, this.tooltip);
    // this.renderer.appendChild(this.el.nativeElement, this.tooltip);

    this.renderer.addClass(this.tooltip, 'ng-tooltip');
    this.renderer.addClass(this.tooltip, `ng-tooltip-${this.placement}`);

    // delay 설정
    this.renderer.setStyle(this.tooltip, '-webkit-transition', `opacity ${this.delay}ms`);
    this.renderer.setStyle(this.tooltip, '-moz-transition', `opacity ${this.delay}ms`);
    this.renderer.setStyle(this.tooltip, '-o-transition', `opacity ${this.delay}ms`);
    this.renderer.setStyle(this.tooltip, 'transition', `opacity ${this.delay}ms`);
  }

  setPosition() {
    // 호스트 요소의 사이즈와 위치 정보
    const hostPos = this.el.nativeElement.getBoundingClientRect();

    // tooltip 요소의 사이즈와 위치 정보
    const tooltipPos = this.tooltip.getBoundingClientRect();

    // window의 scroll top
    // getBoundingClientRect 메소드는 viewport에서의 상대적인 위치를 반환한다.
    // 스크롤이 발생한 경우, tooltip 요소의 top에 세로 스크롤 좌표값을 반영하여야 한다.
    const scrollPos = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop || 0;

    let top, left;

    if (this.placement === 'top') {
      top = hostPos.top - tooltipPos.height - this.offset;
      left = hostPos.left + (hostPos.width - tooltipPos.width) / 2;
    }

    if (this.placement === 'bottom') {
      top = hostPos.bottom + this.offset;
      left = hostPos.left + (hostPos.width - tooltipPos.width) / 2;
    }

    if (this.placement === 'left') {
      top = hostPos.top + (hostPos.height - tooltipPos.height) / 2;
      left = hostPos.left - tooltipPos.width - this.offset;
    }

    if (this.placement === 'right') {
      top = hostPos.top + (hostPos.height - tooltipPos.height) / 2;
      left = hostPos.right + this.offset;
    }

    // 스크롤이 발생한 경우, tooltip 요소의 top에 세로 스크롤 좌표값을 반영하여야 한다.
    this.renderer.setStyle(this.tooltip, 'top', `${top + scrollPos}px`);
    this.renderer.setStyle(this.tooltip, 'left', `${left}px`);
  }
}
/*
<!-- .tooltip-example {
    text-align: center;
    padding: 0 50px;
  }
  .tooltip-example [tooltip] {
    display: inline-block;
    margin: 50px 20px;
    width: 180px;
    height: 50px;
    border: 1px solid gray;
    border-radius: 5px;
    line-height: 50px;
    text-align: center;
  }
  .ng-tooltip {
    position: absolute;
    max-width: 150px;
    font-size: 14px;
    text-align: center;
    color: #f8f8f2;
    padding: 3px 8px;
    background: #282a36;
    border-radius: 4px;
    z-index: 1000;
    opacity: 0;
  }
  .ng-tooltip:after {
    content: "";
    position: absolute;
    border-style: solid;
  }
  .ng-tooltip-top:after {
    top: 100%;
    left: 50%;
    margin-left: -5px;
    border-width: 5px;
    border-color: black transparent transparent transparent;
  }
  .ng-tooltip-bottom:after {
    bottom: 100%;
    left: 50%;
    margin-left: -5px;
    border-width: 5px;
    border-color: transparent transparent black transparent;
  }
  .ng-tooltip-left:after {
    top: 50%;
    left: 100%;
    margin-top: -5px;
    border-width: 5px;
    border-color: transparent transparent transparent black;
  }
  .ng-tooltip-right:after {
    top: 50%;
    right: 100%;
    margin-top: -5px;
    border-width: 5px;
    border-color: transparent black transparent transparent;
  }
  .ng-tooltip-show {
    opacity: 1;
  }*/