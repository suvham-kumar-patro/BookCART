import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellBookComponent } from './sell-book.component';

describe('SellBookComponent', () => {
  let component: SellBookComponent;
  let fixture: ComponentFixture<SellBookComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SellBookComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SellBookComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
