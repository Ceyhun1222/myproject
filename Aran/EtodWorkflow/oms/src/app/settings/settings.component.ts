import { Component, OnInit } from '@angular/core';
import { SettingsService } from '../_services';
import { first, catchError } from 'rxjs/operators';
import { _config } from '../../../projects/omslib/src/lib/_config/_config';
import { Router } from '@angular/router';
import { PublicService } from 'projects/omslib/src/lib/_services/public.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
  slots: any[] = [];
  selectedSlot: any = null;
  selectedPrivateSlot: any = null;
  searchSlot = "";
  loading: boolean = false;
  request: any = {};

  constructor(private router: Router, private publicService: PublicService, private settingsService: SettingsService) { }

  ngOnInit() {
    this.loadSlots();
  }

  loadSlots() {
    this.loading = true;
    this.settingsService.getSlotList().pipe().subscribe(slots => {
      this.loading = false;
      console.log(slots.length)
      if (slots && slots.length > 0) {
        this.slots = slots;
        this.getSelectedSlot();
      }
    });
  }

  getSelectedSlot() {
    this.settingsService.getSelectedSlot().pipe().subscribe(slot => {
      console.log(slot)
      if (slot) {
        this.selectedSlot = slot;

        console.log(this.slots);
        for (let i = 0; i < this.slots.length; i++) {
          const element = this.slots[i];
          if (this.selectedSlot.id === this.slots[i].id) {
            this.selectedSlot.privateSlots = this.slots[i].privateSlots;
          }

        }
      }
    });
  }

  selectSlot(slot) {
    this.selectedSlot = slot;
  }

  selectPrivateSlot(slot) {
    this.selectedSlot.private = slot;
  }

  save() {
    this.request = Object.assign({}, this.selectedSlot);
    delete this.request.privateSlots;
    this.settingsService.slotSave(this.request)
      .pipe(first())
      .subscribe(
        data => {
          console.log(data);
          let user: any = JSON.parse(localStorage.getItem('user')) || JSON.parse(sessionStorage.getItem('user'));
          user.isDefinedSlot = true;
          localStorage.setItem('user', JSON.stringify(user));

          this.publicService.alertMsg("Successfully saved", 1500);
          this.router.navigate(['/users/All']);
        },
        error => {
          console.log(error);
        })
  }
}